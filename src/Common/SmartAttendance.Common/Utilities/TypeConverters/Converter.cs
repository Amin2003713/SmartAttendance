namespace SmartAttendance.Common.Utilities.TypeConverters;

public static class Converter
{
    public static async Task<byte[]> ToByteArray(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    public static Stream ToStream(this byte[] byteArray)
    {
        ArgumentNullException.ThrowIfNull(byteArray);

        return new MemoryStream(byteArray);
    }

    public static double BytesToMegabytes(this long value)
    {
        return value / (1024.0 * 1024.0);
    }

    public static bool TryParsToEnum<TEnum>(this string value, out TEnum result)
        where TEnum : struct
    {
        try
        {
            return Enum.TryParse(value, true, out result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            result = default;
            return false;
        }
    }

    public static bool TryParsToEnum<TEnum>(this string value)
        where TEnum : struct
    {
        try
        {
            return Enum.TryParse<TEnum>(value, true, out _);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public static TEnum ToEnum<TEnum>(this string value)
        where TEnum : struct, Enum
    {
        if (Enum.TryParse<TEnum>(value, true, out var result))
            return result;

        throw new ArgumentException($"'{value}' is not a valid {typeof(TEnum).Name} value.");
    }

    public static List<string> ToList<TEnum>(this TEnum value)
        where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Cast<string>().ToList();
    }

    /// <summary>
    ///     Converts an integer, representing total seconds, to a formatted "HH:MM" string.
    /// </summary>
    /// <param name="totalSeconds">Total seconds as an integer.</param>
    /// <returns>A string formatted as "HH:MM".</returns>
    public static string ToHourMinuteString(this double totalSeconds)
    {
        var timeSpan = TimeSpan.FromSeconds(totalSeconds);
        return $"{(int)timeSpan.TotalHours:D2}:{timeSpan.Minutes:D2}";
    }
}