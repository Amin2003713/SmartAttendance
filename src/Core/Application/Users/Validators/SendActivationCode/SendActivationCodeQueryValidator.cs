using FluentValidation;
using Shifty.Application.Users.Queries.SendActivationCode;
using Shifty.Resources.Messages;

namespace Shifty.Application.Users.Validators.SendActivationCode
{
    public class SendActivationCodeQueryValidator : AbstractValidator<SendActivationCodeQuery>
    {
        public SendActivationCodeQueryValidator(ValidationMessages messages)
        {
            RuleFor(x => x.PhoneNumber).
                NotEmpty().
                WithMessage(messages.PhoneNumber_Required()).
                Matches(@"^09\d{9}$").
                WithMessage(messages.PhoneNumber_InvalidFormat());
        }
    }
}