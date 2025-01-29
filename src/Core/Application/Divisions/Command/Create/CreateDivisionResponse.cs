using System;
using Swashbuckle.AspNetCore.Annotations;

namespace Shifty.Application.Divisions.Command.Create;

/// <summary>
/// Response model for a created division.
/// </summary>
public class CreateDivisionResponse
{
    /// <summary>
    /// Unique identifier for the created division.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the division.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Shift ID associated with the division.
    /// </summary>
    public Guid? ShiftId { get; set; }

    /// <summary>
    /// Parent division ID.
    /// </summary>
    public Guid? ParentId { get; set; }
}