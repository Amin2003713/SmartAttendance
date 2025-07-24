using FluentValidation;
using Shifty.Application.Users.Requests.Commands.Verify;

namespace Shifty.Application.Users.Validators.Commands.Verify;

public class VerifyPhoneNumberRequestValidator : AbstractValidator<VerifyPhoneNumberRequest>
{
    public VerifyPhoneNumberRequestValidator(IStringLocalizer<VerifyPhoneNumberRequestValidator> localizer)
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer["Phone number is required."].Value) // "شماره تلفن الزامی است."
            .Matches(@"^09\d{9}$")
            .WithMessage(localizer["Phone number format is invalid."].Value); // "فرمت شماره تلفن نامعتبر است."

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(localizer["Verification code is required."].Value) // "کد تأیید الزامی است."
            .Length(6)
            .WithMessage(localizer["Verification code must be exactly 6 digits long."]
                .Value); // "کد تأیید باید دقیقاً ۶ رقم باشد."
    }
}