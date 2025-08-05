using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Shifty.Common.Utilities.InjectionHelpers;

namespace Shifty.Persistence.Jwt;

public class JwtService(
    UserManager<User> userManager,
    ILogger<JwtService> logger,
    IStringLocalizer<JwtService> messages,
    IHttpContextAccessor accessor
)
    : IJwtService,
        IScopedDependency
{
    public async Task<AccessToken> GenerateAsync(User user, string uniqueId)
    {
        var secretKey = await accessor.HttpContext!.GenerateShuffledKeyAsync();
        // Load keys securely
        var signingCredentials = new SigningCredentials(secretKey,
            SecurityAlgorithms.HmacSha256Signature,
            SecurityAlgorithms.HmacSha256Signature);


        // Get claims for the user
        var claims = await GetClaimsAsync(user, uniqueId);


        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = ApplicationConstant.JwtSettings.Issuer,
            Audience = ApplicationConstant.JwtSettings.Audience,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow.AddMinutes(ApplicationConstant.JwtSettings.NotBeforeMinutes),
            Expires = DateTime.UtcNow.AddMinutes(ApplicationConstant.JwtSettings.ExpirationMinutes),
            SigningCredentials = signingCredentials,
            Subject = new ClaimsIdentity(claims)
        };

        try
        {
            // Generate JWT
            var tokenHandler  = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

            return new AccessToken(securityToken,
                GenerateRefreshToken(),
                ApplicationConstant.JwtSettings.RefreshTokenValidityInDays);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Source, ex);
            throw IpaException.Unauthorized(messages["Unauthorized Access Token"].Value);
        }
    }

    public async Task<(Guid? userId, Guid? uniq)> ValidateJwtAccessTokenAsync(string token)
    {
        var secretKey = await accessor.HttpContext!.GenerateShuffledKeyAsync();

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = secretKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                },
                out var validatedToken);

            var jwtSecurityToken = (JwtSecurityToken)validatedToken;
            var userId           = Guid.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "id").Value);

            var uni = Guid.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "uniqueTokenIdentifier").Value);
            return (userId, uni);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Source, ex);
            return (null, null);
        }
    }

    private async Task<List<Claim>> GetClaimsAsync(User user, string uniqueId)
    {
        var claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new("username", user.UserName ?? string.Empty),
            new("firstName", user.FirstName ?? string.Empty),
            new("lastName", user.LastName ?? string.Empty),
            new("phoneNumber", user.PhoneNumber ?? string.Empty),
            new("uniqueTokenIdentifier", uniqueId)
        };

        if (!string.IsNullOrEmpty(user.Profile))
            claims.Add(new Claim("profile", user.Profile));

        if (user.LastActionOnServer.HasValue)
            claims.Add(new Claim("lastLoginDate",
                user.LastActionOnServer.Value.ToString("s")));

        // هر نقش را جداگانه به یک Claim تبدیل کنید
        var userRoles = await userManager.GetRolesAsync(user);

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.ToLower()));
        }

        return claims;
    }


    private static string GenerateRefreshToken()
    {
        using var rng       = RandomNumberGenerator.Create();
        var       byteArray = new byte[32];
        rng.GetBytes(byteArray);

        return Convert.ToBase64String(byteArray)
            .Replace("+", string.Empty)
            . // Avoid URL-unsafe characters
            Replace("/",  string.Empty)
            .Replace("=", string.Empty);
    }
}