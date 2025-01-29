using System;

namespace Shifty.Application.Divisions.Requests.Create;

/// <summary>
/// Request model for creating a new division.
/// </summary>
public class CreateDivisionRequest
{
    /// <summary>
    /// Name of the division.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Optional shift ID associated with this division.
    /// </summary>
    public Guid? ShiftId { get; set; }

    /// <summary>
    /// Optional parent division ID (if this division is a sub-division).
    /// </summary>
    public Guid? ParentId { get; set; }
}

