using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shifty.Application.Common.Exceptions;
using Shifty.Common;
using Shifty.Common.General;
using Shifty.Domain.Users;
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
    public class JwtService(IConfiguration configuration , UserManager<User> userManager , ILogger<JwtService> logger) : IJwtService , IScopedDependency
    {
        private readonly SiteSettings _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();


        public async Task<AccessToken> GenerateAsync(User user)
        {
            // Load keys securely
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey) ,
                SecurityAlgorithms.HmacSha256Signature ,
                SecurityAlgorithms.HmacSha256Signature);


            // Get claims for the user
            var claims = await GetClaimsAsync(user);

            // Define token descriptor
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer    = _siteSetting.JwtSettings.Issuer , Audience = _siteSetting.JwtSettings.Audience , IssuedAt = DateTime.UtcNow ,
                NotBefore = DateTime.UtcNow.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes) ,
                Expires   = DateTime.UtcNow.AddMinutes(_siteSetting.JwtSettings.ExpirationMinutes) , SigningCredentials = signingCredentials ,
                Subject   = new ClaimsIdentity(claims) };

            try
            {
                // Generate JWT
                var tokenHandler  = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

                return new AccessToken(securityToken , GenerateRefreshToken() , _siteSetting.JwtSettings.RefreshTokenValidityInDays);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Source , ex);
                throw ShiftyException.Unauthorized(CommonExceptions.Unauthorized_Access);
            }
        }

        public Guid? ValidateJwtAccessTokenAsync(string token)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey); // longer that 16 character

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token ,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true , IssuerSigningKey = new SymmetricSecurityKey(secretKey) , ValidateIssuer = false ,
                        ValidateAudience         = false ,
                        ClockSkew                = TimeSpan.Zero } ,
                    out var validatedToken);

                var jwtSecurityToken = (JwtSecurityToken)validatedToken;
                var userId           = Guid.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value);
                return userId;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Source , ex);
                return null;
            }
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name ,           user.UserName));

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role , role));
            }

            return claims;
        }

        private string GenerateRefreshToken()
        {
            using var rng       = RandomNumberGenerator.Create();
            var       byteArray = new byte[32];
            rng.GetBytes(byteArray);
            return Convert.ToBase64String(byteArray).
                           Replace("+" , string.Empty) // Avoid URL-unsafe characters
                           .
                           Replace("/" , string.Empty).
                           Replace("=" , string.Empty);
        }
    }
}