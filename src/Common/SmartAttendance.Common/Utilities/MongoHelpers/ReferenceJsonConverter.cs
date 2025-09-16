namespace SmartAttendance.Common.Utilities.MongoHelpers;

/// <summary>
///     A JSON converter that serializes all properties of a nested child object
///     except those specified, and deserializes them back into a new instance of T.
/// </summary>
public class ReferenceJsonConverter<T> : JsonConverter<T>
    where T : class
{
    private readonly HashSet<string> _skipProperties = new HashSet<string>(new HashSet<string>
                                                                           {
                                                                               "ProjectId",
                                                                               "IsActive",
                                                                               "CreatedBy",
                                                                               "CreatedAt",
                                                                               "ModifiedBy",
                                                                               "ModifiedAt",
                                                                               "DeletedBy",
                                                                               "DeletedAt"
                                                                           },
                                                                           StringComparer.OrdinalIgnoreCase);

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException($"Expected StartObject token for type {typeToConvert.Name}");

        var instance = Activator.CreateInstance(typeToConvert) as T ??
                       throw new JsonException($"Cannot create instance of {typeToConvert.Name}");

        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return instance;

            if (reader.TokenType != JsonTokenType.PropertyName)
                continue;

            var propName = reader.GetString()!;
            reader.Read();

            if (_skipProperties.Contains(propName))
            {
                reader.Skip();
                continue;
            }

            var prop = props.FirstOrDefault(p => p.Name.Equals(propName, StringComparison.OrdinalIgnoreCase));

            if (prop == null)
            {
                reader.Skip();
                continue;
            }

            var value = JsonSerializer.Deserialize(ref reader, prop.PropertyType, options);
            prop.SetValue(instance, value);
        }

        throw new JsonException($"Incomplete JSON object for type {typeToConvert.Name}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            if (_skipProperties.Contains(prop.Name))
                continue;

            var propValue = prop.GetValue(value);
            writer.WritePropertyName(prop.Name);
            JsonSerializer.Serialize(writer, propValue, prop.PropertyType, options);
        }

        writer.WriteEndObject();
    }
}