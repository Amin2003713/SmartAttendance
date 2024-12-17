using FluentValidation;
using Shifty.Application.Users.Requests;

namespace Shifty.Application.Users.Validators
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
