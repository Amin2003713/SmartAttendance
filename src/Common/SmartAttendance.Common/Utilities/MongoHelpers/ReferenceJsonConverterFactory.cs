namespace SmartAttendance.Common.Utilities.MongoHelpers;

/// <summary>
///     A factory that automatically applies ReferenceJsonConverter for any type with Id/_id and Name properties.
///     Register this once in your JsonSerializerOptions to auto-apply across all models.
/// </summary>
public class ReferenceJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        // Apply globally for any class type that has both Id (or _id) and Name
        if (!typeToConvert.IsClass || typeToConvert == typeof(string) || typeToConvert == typeof(Guid))
            return false;

        var hasId = typeToConvert.GetProperty("Id",  BindingFlags.Public | BindingFlags.Instance) != null ||
                    typeToConvert.GetProperty("_id", BindingFlags.Public | BindingFlags.Instance) != null;

        var hasName = typeToConvert.GetProperty("Name", BindingFlags.Public | BindingFlags.Instance) != null;
        return hasId && hasName;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(ReferenceJsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}