using FluentValidation;
using Shifty.Application.Users.Requests.Commands.Login;
using Shifty.Resources.Messages;

namespace Shifty.Application.Users.Validators.Login
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator(ValidationMessages messages)
        {

            RuleFor(x => x.RefreshToken)
                .NotNull()
                .NotEmpty()
                .WithMessage(messages.Validation_TokenInvalid());

            RuleFor(x => x.AccessToken)
                .NotNull()
                .NotEmpty()
                .WithMessage(messages.Validation_TokenInvalid());
        }
    }
}