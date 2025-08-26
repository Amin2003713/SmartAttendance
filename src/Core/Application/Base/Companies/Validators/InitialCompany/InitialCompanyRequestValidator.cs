using FluentValidation;
using Shifty.Application.Base.Companies.Requests.InitialCompany;

namespace Shifty.Application.Base.Companies.Validators.InitialCompany;

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
        RuleFor(x => x.NationalCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(localizer["National Code is required."].Value) // "رمز عبور الزامی است."
            .MinimumLength(10)
            .WithMessage(localizer["NationalCode must be at least 10 characters long."]
                .Value)
            .MaximumLength(10)
            .WithMessage(localizer["The national code should not be longer than 10 characters."].Value);
    }
}