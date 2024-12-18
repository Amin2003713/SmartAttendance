using FluentValidation;
using Shifty.Application.Users.Requests.Login;

namespace Shifty.Application.Users.Validators.Login
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");

            RuleFor(x => x.AccessToken)
                .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");
        }
    }
}
