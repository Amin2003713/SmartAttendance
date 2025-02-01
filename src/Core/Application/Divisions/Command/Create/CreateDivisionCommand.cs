using System;
using MediatR;

namespace Shifty.Application.Divisions.Command.Create;

/// <summary>
/// MediatR command for creating a division.
/// </summary>
public class CreateDivisionCommand : IRequest
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