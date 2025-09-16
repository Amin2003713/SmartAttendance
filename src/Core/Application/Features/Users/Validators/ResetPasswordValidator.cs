using SmartAttendance.Application.Features.Users.Requests;

namespace SmartAttendance.Application.Features.Users.Validators;

public sealed class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty()
            .WithMessage("ایمیل یا نام کاربری الزامی است.");

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("توکن الزامی است.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("رمز عبور جدید الزامی است.")
            .MinimumLength(8)
            .WithMessage("رمز عبور باید حداقل ۸ کاراکتر باشد.");
    }
}