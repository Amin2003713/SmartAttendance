using App.Applications.Universities.Requests.UpdateCompany;
using App.Common.Utilities.MediaFiles.Validators;
using FluentValidation;

namespace App.Applications.Universities.Validators.UpdateCompany;

/// <summary>
///     Validator class for UpdateUniversityRequest, using FluentValidation.
/// </summary>
public class UpdateUniversityRequestValidator : AbstractValidator<UpdateUniversityRequest>
{
    public UpdateUniversityRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("نام دانشگاه الزامی است")
            .MaximumLength(100)
            .WithMessage("نام دانشگاه نباید بیشتر از ۱۰۰ کاراکتر باشد");

        RuleFor(x => x.LegalName)
            .NotEmpty()
            .WithMessage("نام حقوقی الزامی است")
            .MaximumLength(100)
            .WithMessage("نام حقوقی نباید بیشتر از ۱۰۰ کاراکتر باشد");

 

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("شماره موبایل الزامی است")
            .Matches(@"^(?:\+98|0)?9\d{9}$")
            .WithMessage("شماره موبایل باید معتبر باشد (09XXXXXXXXX یا +989XXXXXXXXX)");

        RuleFor(x => x.PostalCode)
            .Matches(@"^\d{10}$")
            .WithMessage("کد پستی باید یک عدد ۱۰ رقمی باشد")
            .When(x => !string.IsNullOrEmpty(x.PostalCode));

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("آدرس الزامی است")
            .MaximumLength(250)
            .WithMessage("آدرس نباید بیشتر از ۲۵۰ کاراکتر باشد");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("شهر الزامی است");

        RuleFor(x => x.Province)
            .NotEmpty()
            .WithMessage("استان الزامی است");

        RuleFor(x => x.Logo)
            .SetValidator(new UploadMediaFileRequestValidator()!)
            .When(x => x.Logo != null);
    }
}