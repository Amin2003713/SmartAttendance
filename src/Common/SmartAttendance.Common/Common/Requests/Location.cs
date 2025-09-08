using Microsoft.EntityFrameworkCore;

namespace SmartAttendance.Common.Common.Requests;

[Owned]
public record Location(
    double? Lat,
    double? Lng,
    string? Name
);