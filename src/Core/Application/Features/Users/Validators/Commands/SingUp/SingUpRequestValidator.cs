using FluentValidation;
using SmartAttendance.Application.Features.Users.Requests.Commands.SingUp;

namespace SmartAttendance.Application.Features.Users.Validators.Commands.SingUp;

public class SingUpRequestValidator : AbstractValidator<EmployeeSingUpRequest>
{
    public SingUpRequestValidator(IStringLocalizer<SingUpRequestValidator> localizer)
    {
        RuleFor(x => x.FirstName).
            NotEmpty().
            WithMessage(localizer["First name is required."].Value) // "نام کوچک الزامی است."
            .
            Length(2, 255).
            WithMessage(localizer["First name must be between 2 and 255 characters."].Value); // "نام کوچک باید بین ۲ تا ۲۵۵ کاراکتر باشد."

        RuleFor(x => x.LastName).
            NotEmpty().
            WithMessage(localizer["Last name is required."].Value) // "نام خانوادگی الزامی است."
            .
            Length(2, 255).
            WithMessage(localizer["Last name must be between 2 and 255 characters."].Value); // "نام خانوادگی باید بین ۲ تا ۲۵۵ کاراکتر باشد."

        RuleFor(x => x.PhoneNumber).
            NotEmpty().
            WithMessage(localizer["Phone number is required."].Value) // "شماره تلفن الزامی است."
            .
            Matches(@"^09\d{9}$").
            WithMessage(localizer["Phone number format is invalid."].Value); // "فرمت شماره تلفن نامعتبر است."
    }
}