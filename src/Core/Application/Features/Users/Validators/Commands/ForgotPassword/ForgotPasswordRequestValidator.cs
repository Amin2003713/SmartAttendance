using SmartAttendance.Application.Features.Users.Requests.Commands.ForgotPassword;

namespace SmartAttendance.Application.Features.Users.Validators.Commands.ForgotPassword;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator(IStringLocalizer<ForgotPasswordRequestValidator> localizer)
    {
        // Phone number / UserName
        RuleFor(x => x.UserName).
            NotEmpty().
            WithMessage(localizer["Phone number is required."].Value) // "شماره تلفن الزامی است."
            .
            Matches(@"^\d{10,}$").
            WithMessage(localizer["Phone number must have at least 10 digits."].Value); // "شماره تلفن باید حداقل ۱۰ رقم باشد."

        // New Password
        RuleFor(x => x.NewPassword).
            Cascade(CascadeMode.Stop).
            NotEmpty().
            WithMessage(localizer["Password is required."].Value) // "رمز عبور الزامی است."
            .
            MinimumLength(6).
            WithMessage(localizer["Password must be at least 6 characters long."].Value) // "رمز عبور باید حداقل ۶ کاراکتر باشد."
            .
            Matches(@"[A-Z]").
            WithMessage(localizer["Password must contain at least one uppercase letter."].
                            Value) // "رمز عبور باید حداقل یک حرف بزرگ داشته باشد."
            .
            Matches(@"[a-z]").
            WithMessage(localizer["Password must contain at least one lowercase letter."].
                            Value) // "رمز عبور باید حداقل یک حرف کوچک داشته باشد."
            .
            Matches(@"\d").
            WithMessage(localizer["Password must contain at least one digit."].Value) // "رمز عبور باید حداقل یک عدد داشته باشد."
            .
            Matches(@"[\W_]").
            WithMessage(localizer[
                                "Password must contain at least one special character."].
                            Value); // "رمز عبور باید حداقل یک کاراکتر خاص داشته باشد."

        // Confirm Password
        RuleFor(x => x.ConfirmPassword).
            NotEmpty().
            WithMessage(localizer["Confirm password is required."].Value) // "تایید رمز عبور الزامی است."
            .
            Equal(x => x.NewPassword).
            WithMessage(localizer[
                                "Confirm password must match the new password."].
                            Value); // "تایید رمز عبور باید با رمز عبور جدید مطابقت داشته باشد."


        RuleFor(x => x.Code).NotEmpty().WithMessage(localizer["Otp is required."].Value) // "تایید رمز عبور الزامی است."
            ;
    }
}