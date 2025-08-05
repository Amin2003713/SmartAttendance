using Microsoft.EntityFrameworkCore;

namespace Shifty.Common.Common.Requests;

[Owned]
public record Location(
    string Lat,
    string Lng,
    string Name
);