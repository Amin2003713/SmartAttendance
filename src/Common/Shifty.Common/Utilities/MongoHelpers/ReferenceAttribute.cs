namespace Shifty.Common.Utilities.MongoHelpers;

/// <summary>
///     You can annotate any property to emit only its Id and Name via ReferenceJsonConverterFactory.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ReferenceAttribute() : JsonConverterAttribute(typeof(ReferenceJsonConverterFactory));