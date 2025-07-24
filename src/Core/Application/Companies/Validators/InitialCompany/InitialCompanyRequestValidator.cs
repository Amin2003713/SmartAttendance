using FluentValidation;
using Shifty.Application.Companies.Requests.InitialCompany;

namespace Shifty.Application.Companies.Validators.InitialCompany;

public class InitialCompanyRequestValidator : AbstractValidator<InitialCompanyRequest>
{
    public InitialCompanyRequestValidator(IStringLocalizer<InitialCompanyRequestValidator> localizer)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(localizer["First name is required."].Value) // "نام کوچک الزامی است."
            .Length(2, 255)
            .WithMessage(localizer["First name must be between 2 and 255 characters."]
                .Value); // "نام کوچک باید بین ۲ تا ۲۵۵ کاراکتر باشد."

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(localizer["Last name is required."].Value) // "نام خانوادگی الزامی است."
            .Length(2, 255)
            .WithMessage(localizer["Last name must be between 2 and 255 characters."]
                .Value); // "نام خانوادگی باید بین ۲ تا ۲۵۵ کاراکتر باشد."

        RuleFor(x => x.Domain)
            .NotEmpty()
            .WithMessage(localizer["Domain is required."].Value) // "دامنه الزامی است."
            .Matches("^[a-zA-Z0-9-]+$")
            .WithMessage(localizer[
                    "Domain can only contain letters, numbers, and hyphens."]
                .Value); // "دامنه تنها می‌تواند شامل حروف، اعداد و خط تیره باشد."

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localizer["Organization name is required."].Value); // "نام سازمان الزامی است."

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer["Phone number is required."].Value) // "شماره تلفن الزامی است."
            .Matches("^09[0-9]{9}$")
            .WithMessage(localizer["Phone number format is invalid."].Value); // "فرمت شماره تلفن نامعتبر است."


        // New Password
        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(localizer["Password is required."].Value) // "رمز عبور الزامی است."
            .MinimumLength(6)
            .WithMessage(localizer["Password must be at least 6 characters long."]
                .Value) // "رمز عبور باید حداقل ۶ کاراکتر باشد."
            .Matches(@"[A-Z]")
            .WithMessage(localizer["Password must contain at least one uppercase letter."]
                .Value) // "رمز عبور باید حداقل یک حرف بزرگ داشته باشد."
            .Matches(@"[a-z]")
            .WithMessage(localizer["Password must contain at least one lowercase letter."]
                .Value) // "رمز عبور باید حداقل یک حرف کوچک داشته باشد."
            .Matches(@"\d")
            .WithMessage(localizer["Password must contain at least one digit."]
                .Value) // "رمز عبور باید حداقل یک عدد داشته باشد."
            .Matches(@"[\W_]")
            .WithMessage(localizer[
                    "Password must contain at least one special character."]
                .Value); // "رمز عبور باید حداقل یک کاراکتر خاص داشته باشد."

        // Confirm Password
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage(localizer["Confirm password is required."].Value) // "تایید رمز عبور الزامی است."
            .Equal(x => x.Password)
            .WithMessage(localizer[
                    "Confirm password must match the new password."]
                .Value); // "تایید رمز عبور باید با رمز عبور جدید مطابقت داشته باشد."
    }
}