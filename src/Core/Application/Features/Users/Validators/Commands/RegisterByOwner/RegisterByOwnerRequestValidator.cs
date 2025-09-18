using FluentValidation;
using SmartAttendance.Application.Features.Users.Requests.Commands.RegisterByOwner;

namespace SmartAttendance.Application.Features.Users.Validators.Commands.RegisterByOwner;

public class RegisterByOwnerRequestValidator : AbstractValidator<RegisterByOwnerRequest>
{
    public RegisterByOwnerRequestValidator(IStringLocalizer<RegisterByOwnerRequestValidator> localizer)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(localizer["FirstName is required."]);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(localizer["LastName is required."]);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer["PhoneNumber is required."])
            .Matches(@"^\+?\d{10,15}$")
            .WithMessage(localizer["PhoneNumber format is invalid."]);

        RuleFor(x => x.NationalCode)
            .NotEmpty()
            .WithMessage(localizer["NationalCode is required."])
            .Length(10)
            .WithMessage(localizer["NationalCode must be exactly 10 digits."]);

        RuleFor(x => x.PersonalNumber)
            .NotEmpty()
            .WithMessage(localizer["PersonalNumber is required."]);

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Now)
            .When(x => x.BirthDate.HasValue)
            .WithMessage(localizer["BirthDate must be in the past."]);

        RuleFor(x => x.ProfilePicture)
            .Must(file => file == null || file.MediaFile?.Length <= 20 * 1024 * 1024) // max 2MB
            .WithMessage(localizer["Profile image must be smaller than 2MB."]);

        RuleFor(x => x.IsActive)
            .NotNull()
            .WithMessage(localizer["IsActive status must be specified."]);
    }
}