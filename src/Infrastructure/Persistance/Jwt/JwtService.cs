using Shifty.Common;
using Shifty.Common.General;
using Shifty.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shifty.Persistence.Jwt
{
    public class JwtService(IOptionsSnapshot<SiteSettings> settings, UserManager<User> userManager) : IJwtService, IScopedDependency
    {
        private readonly SiteSettings _siteSetting = settings.Value;
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));


       public async Task<AccessToken> GenerateAsync(User user)
{
    
    // Load keys securely
    var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey);
    var signingCredentials = new SigningCredentials(
        new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.HmacSha256Signature);


    // Get claims for the user
    var claims = await GetClaimsAsync(user);

    // Define token descriptor
    var descriptor = new SecurityTokenDescriptor
    {
        Issuer = _siteSetting.JwtSettings.Issuer,
        Audience = _siteSetting.JwtSettings.Audience,
        IssuedAt = DateTime.UtcNow,
        NotBefore = DateTime.UtcNow.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
        Expires = DateTime.UtcNow.AddMinutes(_siteSetting.JwtSettings.ExpirationMinutes),
        SigningCredentials = signingCredentials,
        Subject = new ClaimsIdentity(claims)
    };

    try
    {
        // Generate JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

        return new AccessToken(
            securityToken: securityToken,
            refreshToken: GenerateRefreshToken(),
            refreshTokenExpiresIn: _siteSetting.JwtSettings.RefreshTokenValidityInDays
        );
    }
    catch (Exception ex)
    {
        // Log securely without exposing sensitive information
        throw new SecurityTokenException("Error generating JWT token.", ex);
    }
}

        public Guid? ValidateJwtAccessTokenAsync(string token)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey); // longer that 16 character
            var encryptionKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.EncryptKey); //must be 16 character

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtSecurityToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value);
                return userId;
            }
            catch
            {
                return null;
            }
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private string GenerateRefreshToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var       byteArray = new byte[32];
            rng.GetBytes(byteArray);
            return Convert.ToBase64String(byteArray).
                           Replace("+", string.Empty) // Avoid URL-unsafe characters
                           .
                           Replace("/", string.Empty).
                           Replace("=", string.Empty);
        }
    }
}
