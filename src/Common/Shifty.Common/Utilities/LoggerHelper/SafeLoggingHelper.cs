using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Shifty.Common.Utilities.LoggerHelper;

public static class SafeLoggingHelper
{
    private const int MaxStringLength = 500;

    public static object CreateSafeLogObject<T>(T obj, ILogger? logger = null)
    {
        try

        {
            if (obj == null)
                return new
                    { };

            var type   = typeof(T);
            var result = new Dictionary<string, object?>();


            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propType = prop.PropertyType;
                logger?.LogDebug($"{type} => {prop.Name} ({propType.Name})");

                // Handle known types without calling GetValue unnecessarily
                if (typeof(IFormFile).IsAssignableFrom(propType) ||
                    typeof(IFormFileCollection).IsAssignableFrom(propType))
                {
                    result[prop.Name] = "[BinaryData]";
                    continue;
                }

                if (propType == typeof(byte[]))
                {
                    result[prop.Name] = "[ByteArray]";
                    continue;
                }

                object? value;

                try
                {
                    value = prop.GetValue(obj);
                }
                catch (Exception ex)
                {
                    logger?.LogWarning(ex, "Failed to get value for property {PropName}", prop.Name);
                    result[prop.Name] = "[Error reading property]";
                    continue;
                }

                if (value is null)
                    result[prop.Name] = null;
                else if (value is string strVal && strVal.Length > MaxStringLength)
                    result[prop.Name] = $"{strVal.Substring(0, MaxStringLength)}... (truncated)";
                else
                    result[prop.Name] = value;
            }

            return result;
        }
        catch (Exception e)

        {
            return null!;
        }
    }
}