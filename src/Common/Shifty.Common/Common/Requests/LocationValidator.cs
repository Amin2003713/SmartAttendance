using FluentValidation;

namespace Shifty.Common.Common.Requests;

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