using FluentValidation;
using Shifty.Application.Users.Requests.Verify;

namespace Shifty.Application.Users.Validators.Verify;

public class VerifyPhoneNumberRequestValidator : AbstractValidator<VerifyPhoneNumberRequest>
{
    public VerifyPhoneNumberRequestValidator()
    {
        RuleFor(x => x.PhoneNumber).
            NotEmpty().
            WithMessage("Phone number is required.")
            .Matches(@"^09\d{9}$").
            WithMessage("Invalid phone number format. Use international format (e.g., 09131234567).");

        RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required.").Length(6).WithMessage("Code must be 6 digits.");
    }
}