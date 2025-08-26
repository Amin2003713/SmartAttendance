using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Shifty.Common.Common.Requests;

[Owned]
public record Location(
    double? Lat,
    double? Lng,
    string? Name
);

