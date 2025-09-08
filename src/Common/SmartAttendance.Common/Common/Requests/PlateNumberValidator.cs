using FluentValidation;

namespace SmartAttendance.Common.Common.Requests;

public class PlateNumberValidator : AbstractValidator<PlateNumber>
{
    public PlateNumberValidator()
    {
        RuleFor(x => x.RightNumber)
            .NotEmpty()
            .WithMessage("Right number is required.")
            .Matches(@"^\d{3}$")
            .WithMessage("Right number must be exactly 3 digits.");

        RuleFor(x => x.MiddleMark)
            .NotEmpty()
            .WithMessage("Middle mark is required.")
            .Length(1)
            .WithMessage("Middle mark must be a single character.");

        RuleFor(x => x.LeftNumber)
            .NotEmpty()
            .WithMessage("Left number is required.")
            .Matches(@"^\d{2}$")
            .WithMessage("Left number must be exactly 2 digits.");

        RuleFor(x => x.RegionCode)
            .NotEmpty()
            .WithMessage("Region code is required.")
            .Matches(@"^\d{2,3}$")
            .WithMessage("Region code must be 2 or 3 digits.");
    }
}