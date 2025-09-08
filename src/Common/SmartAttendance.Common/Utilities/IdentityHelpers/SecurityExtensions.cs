using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace SmartAttendance.Common.Utilities.IdentityHelpers;

public static class SecurityExtensions
{
    public static async Task<SymmetricSecurityKey> GenerateShuffledKeyAsync(this HttpContext context)
    {
        // ۱. دستیابی به HybridCache
        var cache = context.RequestServices.GetRequiredService<HybridCache>();

        // ۲. کلید یکتا برای tenant+IP
        var tenant = context.Request.Headers.TryGetValue("X-Tenant", out var t)
            ? t.ToString()
            : "DefaultTenant";

        var cacheKey = $"ShuffledKey:{tenant}";

        // ۳. GetOrCreate: در صورت کش‌میس، HMAC اجرا می‌شود
        var keyBytes = await cache.GetOrCreateAsync(cacheKey,
            token => CreateKey("0.0.0.0", tenant, token),
            cancellationToken: context.RequestAborted);

        return keyBytes;
    }

    private static ValueTask<SymmetricSecurityKey> CreateKey(string ip, string tenant, CancellationToken token)
    {
        // محاسبه هش IP
        var ipHash = ComputeSha256Hash(ip);

        // ترکیب کلید با IP هش شده
        var combinedKey = tenant + ApplicationConstant.JwtSettings.SecretKey + ipHash;

        // اعمال تغییرات بیت‌به‌بیت و شافل کردن داده‌ها
        var shuffledKey = combinedKey.ToCharArray();

        for (var i = 0; i < shuffledKey.Length; i++)
        {
            shuffledKey[i] = (char)(shuffledKey[i] ^ ipHash[i % ipHash.Length]); // XOR با IP Hash
            shuffledKey[i] = (char)(shuffledKey[i] << 1 | shuffledKey[i] >> 7);  // Bitwise shift
        }

        // هش نهایی برای امنیت بیشتر
        using var sha256 = SHA256.Create();

        return new ValueTask<SymmetricSecurityKey>(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new string(shuffledKey))));
    }

    private static string ComputeSha256Hash(string rawData)
    {
        using var sha256 = SHA256.Create();
        var       bytes  = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return Convert.ToBase64String(bytes);
    }
}