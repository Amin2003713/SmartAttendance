using FluentValidation;
using Shifty.Application.Companies.Requests;
using Shifty.Resources.Messages;

namespace Shifty.Application.Companies.Validators.InitialCompany
{
    public class InitialCompanyRequestValidator : AbstractValidator<InitialCompanyRequest>
    {
        public InitialCompanyRequestValidator(ValidationMessages validationMessages)
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(validationMessages.FirstName_Required()).Length(2 , 255).WithMessage(validationMessages.FirstName_Length());

            RuleFor(x => x.LastName).NotEmpty().WithMessage(validationMessages.LastName_Required()).Length(2 , 255).WithMessage(validationMessages.LastName_Length());

            RuleFor(x => x.Domain).
                NotEmpty().
                WithMessage(validationMessages.Domain_Required()).
                Matches("^[a-zA-Z0-9-]+$").
                WithMessage(validationMessages.Domain_InvalidCharacters());

            RuleFor(x => x.Name).NotEmpty().WithMessage(validationMessages.OrganizationName_Required());

            RuleFor(x => x.PhoneNumber).
                NotEmpty().
                WithMessage(validationMessages.PhoneNumber_Required()).
                Matches("^09[0-9]{9}$").
                WithMessage(validationMessages.PhoneNumber_InvalidFormat());
        }
    }
}