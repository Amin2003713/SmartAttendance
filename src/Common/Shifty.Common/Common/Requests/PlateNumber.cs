using Microsoft.EntityFrameworkCore;

namespace Shifty.Common.Common.Requests;

[Owned]
public record PlateNumber(string? LeftNumber, string? MiddleMark, string? RightNumber, string? RegionCode);