using FluentValidation;
using Shifty.Application.Companies.Requests;

namespace Shifty.Application.Companies.Validators
{
    public class InitialCompanyRequestValidator : AbstractValidator<InitialCompanyRequest>
    {
        public InitialCompanyRequestValidator()
        {
            // Name is required and should not exceed 150 characters
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("نام  الزامی است.").MaximumLength(150).WithMessage("نام  نباید بیشتر از 150 کاراکتر باشد.");


            RuleFor(x => x.LastName).
                NotEmpty().
                WithMessage("نام خانوادگی الزامی است.").
                MaximumLength(150).
                WithMessage("نام خانوادگی نباید بیشتر از 150 کاراکتر باشد.");

            // TenantId is required and must match a valid format
            RuleFor(x => x.Domain).
                NotEmpty().
                WithMessage("شناسه سازمان الزامی است.").
                Matches("^[a-zA-Z0-9-]+$").
                WithMessage("شناسه سازمان فقط می‌تواند شامل حروف الفبا، اعداد، و خط فاصله باشد.");

            RuleFor(x => x.Name).NotEmpty().WithMessage("نام سازمان الزامی است.");

            // // Address is optional but must not exceed 250 characters if provided
            // RuleFor(x => x.Address)
            //     .MaximumLength(250).WithMessage("آدرس نباید بیشتر از 250 کاراکتر باشد.");
            //
            // // PostalCode should be 10 digits if provided
            // RuleFor(x => x.PostalCode)
            //     .Matches("^[0-9]{10}$").When(x => !string.IsNullOrEmpty(x.PostalCode))
            //     .WithMessage("کد پستی باید دقیقا 10 رقم باشد.");

            // PhoneNumber should follow Iranian phone number format
            RuleFor(x => x.PhoneNumber).
                NotEmpty().
                WithMessage("شماره تلفن الزامی است.").
                Matches("^09[0-9]{9}$").
                WithMessage("شماره تلفن باید مطابق با فرمت شماره تلفن ایران باشد.");
        }
    }
}