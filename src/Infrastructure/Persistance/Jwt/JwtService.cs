using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shifty.Common;
using Shifty.Common.General;
using Shifty.Resources.Messages;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shifty.Common.Exceptions;
using Shifty.Common.Utilities;
using Shifty.Domain.Features.Users;

namespace Shifty.Persistence.Jwt
{
    public class JwtService(UserManager<User> userManager , ILogger<JwtService> logger , CommonMessages messages , IHttpContextAccessor accessor) : IJwtService , IScopedDependency
    {
        public async Task<AccessToken> GenerateAsync(User user)
        {
            var secretKey = accessor.HttpContext.GenerateShuffledKey();
            // Load keys securely
            var signingCredentials = new SigningCredentials(secretKey ,
                SecurityAlgorithms.HmacSha256Signature ,
                SecurityAlgorithms.HmacSha256Signature);


            // Get claims for the user
            var claims = await GetClaimsAsync(user);

            // Define token descriptor
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer    = ApplicationConstant.JwtSettings.Issuer ,
                Audience = ApplicationConstant.JwtSettings.Audience ,
                IssuedAt = DateTime.UtcNow ,
                NotBefore = DateTime.UtcNow.AddMinutes(ApplicationConstant.JwtSettings.NotBeforeMinutes) ,
                Expires   = DateTime.UtcNow.AddMinutes(ApplicationConstant.JwtSettings.ExpirationMinutes) , SigningCredentials = signingCredentials ,
                Subject   = new ClaimsIdentity(claims) };

            try
            {
                // Generate JWT
                var tokenHandler  = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

                return new AccessToken(securityToken , GenerateRefreshToken() , ApplicationConstant.JwtSettings.RefreshTokenValidityInDays);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Source , ex);
                throw ShiftyException.Unauthorized(messages.Unauthorized_Access());
            }
        }

        public Guid? ValidateJwtAccessTokenAsync(string token)
        {
            var secretKey = accessor.HttpContext.GenerateShuffledKey();

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token ,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true , IssuerSigningKey = secretKey , ValidateIssuer = false ,
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
            var claims = new List<Claim>
            {
                new Claim("id" ,        user.Id.ToString()) ,
                new Claim("username" ,  user.UserName  ?? string.Empty) ,
                new Claim("firstName" , user.FirstName ?? string.Empty) ,
                new Claim("lastName" ,  user.LastName  ?? string.Empty) ,
            };

            if (!string.IsNullOrEmpty(user.Profile))
                claims.Add(new Claim("profile" , user.Profile));

            if(user.LastLoginDate.HasValue)
                claims.Add(new Claim("lastLoginDate" , user.LastLoginDate.Value.ToString("s")));

            // Add roles from the user manager
            var userRoles = await userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role , role)));

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

