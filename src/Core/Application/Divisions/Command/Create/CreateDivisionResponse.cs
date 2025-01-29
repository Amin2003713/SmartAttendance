using System;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Response model for a created division.
/// </summary>
public class CreateDivisionResponse
{
    /// <summary>
    /// Unique identifier for the created division.
    /// </summary>
    [SwaggerSchema("شناسه یکتا بخش")]
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the division.
    /// </summary>
    [SwaggerSchema("نام بخش")]
    public string Name { get; set; }

    /// <summary>
    /// Shift ID associated with the division.
    /// </summary>
    [SwaggerSchema("شناسه شیفت")]
    public Guid? ShiftId { get; set; }

    /// <summary>
    /// Parent division ID.
    /// </summary>
    [SwaggerSchema("شناسه بخش والد")]
    public Guid? ParentId { get; set; }
}