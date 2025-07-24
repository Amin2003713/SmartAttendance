using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Shifty.Common.Common.Requests;

[Owned]
public record Location(
    string Lat,
    string Lng,
    string Name
);

public class LocationValidator : AbstractValidator<Location>
{
    public LocationValidator()
    {
        RuleFor(x => x.Lat)
            .NotEmpty()
            .WithMessage("Latitude is required.")
            .Matches(@"^-?\d+(\.\d+)?$")
            .WithMessage("Invalid latitude format.");

        RuleFor(x => x.Lng)
            .NotEmpty()
            .WithMessage("Longitude is required.")
            .Matches(@"^-?\d+(\.\d+)?$")
            .WithMessage("Invalid longitude format.");
    }
}