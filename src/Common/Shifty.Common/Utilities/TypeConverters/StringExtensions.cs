namespace Shifty.Common.Utilities.TypeConverters;

public static class StringExtensions
{
    public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
    {
        return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
    }

    public static int ToInt(this string value)
    {
        return Convert.ToInt32(value);
    }

    public static decimal ToDecimal(this string value)
    {
        return Convert.ToDecimal(value);
    }

    public static string ToNumeric(this int value)
    {
        return value.ToString("N0"); //"123,456"
    }

    public static string ToNumeric(this decimal value)
    {
        return value.ToString("N0");
    }

    public static string ToCurrency(this int value)
    {
        //fa-IR => current culture currency symbol => ریال
        //123456 => "123,123ریال"
        return value.ToString("C0");
    }

    public static string ToCurrency(this decimal value)
    {
        return value.ToString("C0");
    }

    public static string NullIfEmpty(this string str)
    {
        return str?.Length == 0 ? null : str;
    }

    public static string? BuildImageUrl(this string url, bool compress = false)
    {
        return string.IsNullOrEmpty(url)
            ? null
            : compress
                ? $"https://{url!.Replace("&compress=False", "&compress=True")}"
                : $"https://{url}";
    }

    public static List<string?> BuildImageUrls(this IEnumerable<string?> urls)
    {
        if (urls == null) return new List<string?>();

        return urls.Select(url =>
            {
                if (string.IsNullOrWhiteSpace(url))
                    return null;

                var shouldCompress = url.Contains("Picture", StringComparison.OrdinalIgnoreCase);

                return url.BuildImageUrl(shouldCompress);
            })
            .ToList();
    }
}