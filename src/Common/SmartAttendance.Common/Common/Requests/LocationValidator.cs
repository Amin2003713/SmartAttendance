using FluentValidation;

namespace SmartAttendance.Common.Common.Requests;

public class LocationValidator : AbstractValidator<Location>
{
    public LocationValidator()
    {
        RuleFor(x => x.Lat.ToString()).NotEmpty().WithMessage("Latitude is required.").Matches(@"^-?\d+(\.\d+)?$").WithMessage("Invalid latitude format.");

        RuleFor(x => x.Lng.ToString()).NotEmpty().WithMessage("Longitude is required.").Matches(@"^-?\d+(\.\d+)?$").WithMessage("Invalid longitude format.");
    }
}