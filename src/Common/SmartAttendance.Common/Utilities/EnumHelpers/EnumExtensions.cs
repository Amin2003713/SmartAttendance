using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace SmartAttendance.Common.Utilities.EnumHelpers;

public static class EnumExtensions
{
    /// <summary>
    ///     دریافت نام نمایشی (فارسی) مقدار Enum از DisplayAttribute
    /// </summary>
    public static string GetDisplayName(this Enum value)
    {
        var field     = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DisplayAttribute>();

        return attribute?.Name ??
               value.ToString().ToLower();
    }

    /// <summary>
    ///     دریافت نام اصلی مقدار Enum (انگلیسی)
    /// </summary>
    public static string GetEnglishName(this Enum value)
    {
        return value.ToString();
    }

    /// <summary>
    ///     دریافت نام نمایشی (فارسی) و نام اصلی (انگلیسی) مقدار Enum
    /// </summary>
    public static string GetEnumNames(this Enum value)
    {
        var usePersian =
            CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Contains("IR", StringComparison.OrdinalIgnoreCase);

        return usePersian ? value.GetDisplayName() : value.GetEnglishName().ToLower();
    }


    /// <summary>
    ///     Reads out each set flag’s DisplayAttribute.Name (if any), or its enum identifier.
    /// </summary>
    public static IEnumerable<string> GetDisplayNames<TEnum>(this TEnum composite)
        where TEnum : Enum
    {
        var enumType = typeof(TEnum);

        foreach (var flag in composite.GetFlags())
        {
            var member = enumType.GetMember(flag.ToString()).FirstOrDefault();

            if (member != null)
            {
                var disp = member.GetCustomAttribute<DisplayAttribute>();

                if (disp != null && !string.IsNullOrEmpty(disp.Name))
                    yield return disp.Name;
            }

            // fallback to the enum's name
            yield return flag.ToString();
        }
    }


    /// <summary>
    ///     Returns each individual flag that is set on the composite enum value.
    ///     Skips the zero value.
    /// </summary>
    public static IEnumerable<TEnum> GetFlags<TEnum>(this TEnum composite)
        where TEnum : Enum
    {
        var enumType = typeof(TEnum);

        foreach (var value in Enum.GetValues(enumType).Cast<TEnum>())
        {
            var longValue = Convert.ToInt64(value);

            if (longValue == 0)
                continue;

            if (composite.HasFlag(value))
                yield return value;
        }
    }


    public static List<TEnum> ToList<TEnum>()
        where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
    }

    public static Dictionary<string, int> ToDictionary<TEnum>()
        where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).
                    Cast<TEnum>().
                    ToDictionary(
                        key => key.GetEnumNames(),
                        value => Convert.ToInt32(value)
                    );
    }


    public static FileType GetFileType(this string ext)
    {
        return ext switch

               {
                   "pdf" => FileType.Pdf, "zip" => FileType.Zip, _ => FileType.Picture
               };
    }

    public static FileType GetFileType(this IFormFile file)
    {
        return file.FileExtension() switch
               {
                   "pdf" => FileType.Pdf, "zip" => FileType.Zip, _ => FileType.Picture
               };
    }


    public static string FileExtension(this IFormFile File)
    {
        return (Path.GetExtension(File?.FileName) ?? string.Empty).Replace(".", "");
    }
}