using FluentValidation;
using Shifty.Application.Features.Users.Requests.Commands.Login;

namespace Shifty.Application.Features.Users.Validators.Commands.Login;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator(IStringLocalizer<LoginRequestValidator> localizer)
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage(localizer["Username is required."].Value); // "نام کاربری الزامی است."

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(localizer["Password is required."].Value) // "رمز عبور الزامی است."
            .MinimumLength(8)
            .WithMessage(localizer["Password must be at least 8 characters long."]
                .Value); // "رمز عبور باید حداقل ۸ کاراکتر باشد."
    }
}