using FluentValidation;
using Shifty.ApplicationLogic.Users.Requests;

namespace Shifty.ApplicationLogic.Users.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.Password)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");
        }
    }
}
