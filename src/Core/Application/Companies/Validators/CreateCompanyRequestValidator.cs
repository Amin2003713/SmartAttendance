using FluentValidation;
using Shifty.Application.Companies.Requests;
using System;

namespace Shifty.Application.Companies.Validators;

public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestValidator()
{
    // Name is required and should not exceed 150 characters
    RuleFor(x => x.Name)
        .NotEmpty().WithMessage("نام شرکت الزامی است.")
        .MaximumLength(150).WithMessage("نام شرکت نباید بیشتر از 150 کاراکتر باشد.");

    // TenantId is required and must match a valid format
    RuleFor(x => x.Identifier)
        .NotEmpty().WithMessage("شناسه شرکت الزامی است.")
        .Matches("^[a-zA-Z0-9-]+$").WithMessage("شناسه شرکت فقط می‌تواند شامل حروف الفبا، اعداد، و خط فاصله باشد.");

    // NationalId should be 10 digits
    RuleFor(x => x.NationalId)
        .NotEmpty().WithMessage("کد ملی الزامی است.")
        .Length(10).WithMessage("کد ملی باید دقیقا 10 رقم باشد.")
        .Matches("^[0-9]+$").WithMessage("کد ملی باید فقط شامل ارقام باشد.");

    // RegistrationNumber is required
    RuleFor(x => x.RegistrationNumber)
        .NotEmpty().WithMessage("شماره ثبت الزامی است.");

    // EconomicCode should be 12 digits
    RuleFor(x => x.EconomicCode)
        .NotEmpty().WithMessage("کد اقتصادی الزامی است.")
        .Length(12).WithMessage("کد اقتصادی باید دقیقا 12 رقم باشد.")
        .Matches("^[0-9]+$").WithMessage("کد اقتصادی باید فقط شامل ارقام باشد.");

    // Address is optional but must not exceed 250 characters if provided
    RuleFor(x => x.Address)
        .MaximumLength(250).WithMessage("آدرس نباید بیشتر از 250 کاراکتر باشد.");

    // PostalCode should be 10 digits if provided
    RuleFor(x => x.PostalCode)
        .Matches("^[0-9]{10}$").When(x => !string.IsNullOrEmpty(x.PostalCode))
        .WithMessage("کد پستی باید دقیقا 10 رقم باشد.");

    // PhoneNumber should follow Iranian phone number format
    RuleFor(x => x.PhoneNumber)
        .NotEmpty().WithMessage("شماره تلفن الزامی است.")
        .Matches("^09[0-9]{9}$").WithMessage("شماره تلفن باید مطابق با فرمت شماره تلفن ایران باشد.");

    // Email should be a valid email address
    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("آدرس ایمیل الزامی است.")
        .EmailAddress().WithMessage("آدرس ایمیل وارد شده معتبر نیست.");

    RuleFor(x => x.UserId).NotEqual(Guid.Empty)
                          .WithMessage("شناسه کاربری الزامی است.");
}

}