using FluentValidation;
using Shifty.Application.Users.Requests.Commands.Verify;
using Shifty.Resources.Messages;

namespace Shifty.Application.Users.Validators.Verify
{
    public class VerifyPhoneNumberRequestValidator : AbstractValidator<VerifyPhoneNumberRequest>
    {
        public VerifyPhoneNumberRequestValidator(ValidationMessages messages)
        {
            RuleFor(x => x.PhoneNumber).
                NotEmpty().
                WithMessage(messages.PhoneNumber_Required()).
                Matches(@"^09\d{9}$").
                WithMessage(messages.PhoneNumber_InvalidFormat());

            RuleFor(x => x.Code).NotEmpty().WithMessage(messages.Code_Required()).Length(6).WithMessage(messages.Code_Length());
        }
    }
}