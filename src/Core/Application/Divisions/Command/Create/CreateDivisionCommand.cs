using System;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// MediatR command for creating a division.
/// </summary>
public class CreateDivisionCommand : IRequest<CreateDivisionResponse>
{
    /// <summary>
    /// Name of the division.
    /// </summary>
    [SwaggerSchema("نام بخش جدید", Nullable = false)]
    public string Name { get; set; }

    /// <summary>
    /// Optional shift ID associated with this division.
    /// </summary>
    [SwaggerSchema("شناسه شیفت اختیاری")]
    public Guid? ShiftId { get; set; }

    /// <summary>
    /// Optional parent division ID (if this division is a sub-division).
    /// </summary>
    [SwaggerSchema("شناسه بخش والد (در صورت وجود)")]
    public Guid? ParentId { get; set; }
}