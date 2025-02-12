using FluentValidation;
using Shifty.Application.Users.Requests.Commands.Login;
using Shifty.Resources.Messages;

namespace Shifty.Application.Users.Validators.Login
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator(ValidationMessages messages)
        {

            RuleFor(x => x.RefreshToken).NotNull().NotEmpty().WithName(messages.Property_RefreshToken()).WithMessage(messages.Validation_TokenInvalid());

            RuleFor(x => x.AccessToken).NotNull().NotEmpty().WithName(messages.Property_AccessToken()).WithMessage(messages.Validation_TokenInvalid());
        }
    }
}