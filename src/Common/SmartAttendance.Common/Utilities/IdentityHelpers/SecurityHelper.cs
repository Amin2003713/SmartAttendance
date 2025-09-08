namespace SmartAttendance.Common.Utilities.IdentityHelpers;

public static class SecurityHelper
{
    public static string ComputeSha256Hash(this string input)
    {
        // ۱. تبدیل رشته به بایت (UTF8)
        var bytes = Encoding.UTF8.GetBytes(input);

        // ۲. شیء SHA256 بسازید و هش را محاسبه کنید
        using var sha256    = SHA256.Create();
        var       hashBytes = sha256.ComputeHash(bytes);

        // ۳. تبدیل بایت‌های هش به رشته هگزادسیمال
        var builder = new StringBuilder();

        foreach (var b in hashBytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }


    public static string GetSha256Hash(string input)
    {
        using var sha256    = SHA256.Create();
        var       byteValue = Encoding.UTF8.GetBytes(input);
        var       byteHash  = sha256.ComputeHash(byteValue);
        return Convert.ToBase64String(byteHash);
        //return BitConverter.ToString(byteHash).Replace("-", "").ToLower();
    }
}