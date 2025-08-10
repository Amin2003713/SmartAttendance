using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Shifty.Common.Common.Requests;

[Owned]
public record Location(
    double? Lat,
    double? Lng,
    string? Name
);

public class LocationsValidator : AbstractValidator<Location>
{
    public LocationsValidator()
    {
        RuleFor(x => x.Lat)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90.");

        RuleFor(x => x.Lng)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180.");
    }
}