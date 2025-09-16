using SmartAttendance.Application.Features.Users.Requests;

namespace SmartAttendance.Application.Features.Users.Validators;

public sealed class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty()
            .WithMessage("ایمیل یا نام کاربری الزامی است.")
            .MinimumLength(3)
            .WithMessage("ایمیل/نام کاربری نامعتبر است.");
    }
}