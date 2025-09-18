using FluentValidation;
using SmartAttendance.Application.Features.Users.Requests.Commands.UpdateUser;

namespace SmartAttendance.Application.Features.Users.Validators.Commands.UpdateUser;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("FirstName is required.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("LastName is required.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber is required.")
            .Matches(@"^\+?\d{10,15}$")
            .WithMessage("PhoneNumber format is invalid.");

        RuleFor(x => x.NationalCode)
            .NotEmpty()
            .WithMessage("NationalCode is required.")
            .Length(10)
            .WithMessage("NationalCode must be exactly 10 digits.");

        RuleFor(x => x.PersonalNumber)
            .NotEmpty()
            .WithMessage("PersonalNumber is required.");

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Now)
            .When(x => x.BirthDate.HasValue)
            .WithMessage("BirthDate must be in the past.");

        RuleFor(x => x.ProfilePicture)
            .Must(file => file == null || file.MediaFile?.Length <= 2 * 1024 * 1024) // max 2MB
            .WithMessage("Profile image must be smaller than 2MB.");

        RuleFor(x => x.IsActive)
            .NotNull()
            .WithMessage("IsActive status must be specified.");
    }
}