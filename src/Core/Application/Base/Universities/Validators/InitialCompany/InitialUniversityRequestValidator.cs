using FluentValidation;
using SmartAttendance.Application.Base.Universities.Requests.InitialUniversity;

namespace SmartAttendance.Application.Base.Universities.Validators.InitialCompany;

public class InitialUniversityRequestValidator : AbstractValidator<InitialUniversityRequest>
{
    public InitialUniversityRequestValidator(IStringLocalizer<InitialUniversityRequestValidator> localizer)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(localizer["First name is required."].Value) // "نام کوچک الزامی است."
            .Length(2, 255)
            .WithMessage(localizer["First name must be between 2 and 255 characters."].Value); // "نام کوچک باید بین ۲ تا ۲۵۵ کاراکتر باشد."

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(localizer["Last name is required."].Value) // "نام خانوادگی الزامی است."
            .Length(2, 255)
            .WithMessage(localizer["Last name must be between 2 and 255 characters."].Value); // "نام خانوادگی باید بین ۲ تا ۲۵۵ کاراکتر باشد."

        RuleFor(x => x.Domain)
            .NotEmpty()
            .WithMessage(localizer["Domain is required."].Value) // "دامنه الزامی است."
            .Matches("^[a-zA-Z0-9-]+$")
            .WithMessage(localizer[
                "Domain can only contain letters, numbers, and hyphens."].Value); // "دامنه تنها می‌تواند شامل حروف، اعداد و خط تیره باشد."

        RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["Organization name is required."].Value); // "نام سازمان الزامی است."

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer["Phone number is required."].Value) // "شماره تلفن الزامی است."
            .Matches("^09[0-9]{9}$")
            .WithMessage(localizer["Phone number format is invalid."].Value); // "فرمت شماره تلفن نامعتبر است."
    }
}