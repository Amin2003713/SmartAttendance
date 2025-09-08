using Microsoft.EntityFrameworkCore;

namespace SmartAttendance.Common.Common.Requests;

[Owned]
public record PlateNumber(
    string? LeftNumber,
    string? MiddleMark,
    string? RightNumber,
    string? RegionCode
);