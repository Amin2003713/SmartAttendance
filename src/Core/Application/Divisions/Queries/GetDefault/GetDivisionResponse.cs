using System;
using Swashbuckle.AspNetCore.Annotations;

namespace Shifty.Application.Divisions.Queries.GetDefault;

/// <summary>
/// Response model for retrieving a division.
/// </summary>
public class GetDivisionResponse
{

    public Guid Id { get; set; }
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