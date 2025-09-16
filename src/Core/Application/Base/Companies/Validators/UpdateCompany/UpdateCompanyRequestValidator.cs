using SmartAttendance.Application.Base.Companies.Requests.UpdateCompany;
using SmartAttendance.Application.Commons.MediaFiles.Validators;

namespace SmartAttendance.Application.Base.Companies.Validators.UpdateCompany;

/// <summary>
///     Validator class for , using FluentValidation.
/// </summary>
public class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
{
    public UpdateCompanyRequestValidator(IStringLocalizer<UpdateCompanyRequestValidator> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localizer["Company name is required"].Value)
            .MaximumLength(100)
            .WithMessage(localizer["Company name must not exceed 100 characters"].Value);

        RuleFor(x => x.LegalName)
            .NotEmpty()
            .WithMessage(localizer["Legal name is required"].Value)
            .MaximumLength(100)
            .WithMessage(localizer["Legal name must not exceed 100 characters"].Value);

        RuleFor(x => x.NationalCode)
            .NotEmpty()
            .WithMessage(localizer["National Code is required"].Value)
            .Matches(@"^\d{10}$")
            .WithMessage(localizer["National Code must be a 10-digit number"].Value);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer["Phone number is required"].Value)
            .Matches(@"^(?:\+98|0)?9\d{9}$")
            .WithMessage(localizer["Phone number must be a valid Iranian mobile number (09XXXXXXXXX or +989XXXXXXXXX)"].Value);

        RuleFor(x => x.PostalCode)
            .Matches(@"^\d{10}$")
            .WithMessage(localizer["Zip code must be a 10-digit number"].Value)
            .When(x => !string.IsNullOrEmpty(x.PostalCode));

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage(localizer["Address is required"].Value)
            .MaximumLength(250)
            .WithMessage(localizer["Address must not exceed 250 characters"].Value);

        RuleFor(x => x.City).NotEmpty().WithMessage(localizer["City is required"].Value);

        RuleFor(x => x.Province).NotEmpty().WithMessage(localizer["Province is required"].Value);

        RuleFor(x => x.Logo).SetValidator(new UploadMediaFileRequestValidator()!).When(x => x.Logo != null);
    }
}