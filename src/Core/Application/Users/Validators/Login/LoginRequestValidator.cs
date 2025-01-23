using FluentValidation;
using Shifty.Application.Users.Requests.Login;
using Shifty.Resources.Messages;

namespace Shifty.Application.Users.Validators.Login
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator(ValidationMessages messages)
        {

            RuleFor(x => x.Username).NotEmpty().WithMessage(messages.Username_Required());

            RuleFor(x => x.Password).NotEmpty().WithMessage(messages.Password_Required()).MinimumLength(8).WithMessage(messages.Password_MinLength());
        }
    }
}