using FluentValidation;
using Shifty.Application.Users.Requests.SendActivationCode;

namespace Shifty.Application.Users.Validators.SendActivationCode
{
    public class SendActivationCodeValidator : AbstractValidator<SendActivationCodeRequest>
    {

        public SendActivationCodeValidator() =>
            RuleFor(a=>a.UserId).NotEmpty().WithMessage("UserId cannot be empty");
    }
}