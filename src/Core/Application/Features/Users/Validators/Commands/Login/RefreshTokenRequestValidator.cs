using FluentValidation;
using SmartAttendance.Application.Features.Users.Requests.Commands.Login;

namespace SmartAttendance.Application.Features.Users.Validators.Commands.Login;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator(IStringLocalizer<RefreshTokenRequestValidator> localizer)
    {
        RuleFor(x => x.RefreshToken)
            .NotNull()
            .NotEmpty()
            .WithMessage(localizer["Invalid or missing token."].Value); // "توکن نامعتبر یا مفقود است."

        RuleFor(x => x.AccessToken)
            .NotNull()
            .NotEmpty()
            .WithMessage(localizer["Invalid or missing token."].Value); // "توکن نامعتبر یا مفقود است."
    }
}