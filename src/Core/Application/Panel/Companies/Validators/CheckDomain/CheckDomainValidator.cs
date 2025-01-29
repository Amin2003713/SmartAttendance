using FluentValidation;
using Shifty.Application.Panel.Companies.Queries.CheckDomain;
using Shifty.Resources.Messages;

namespace Shifty.Application.Panel.Companies.Validators.CheckDomain
{
    public class CheckDomainValidator  : AbstractValidator<CheckDomainQuery>
    {
        public CheckDomainValidator(ValidationMessages messages)
        {
            RuleFor(x => x.Domain).
                Cascade(CascadeMode.Stop) // Stop validation on first failure
                .NotEmpty().
                WithMessage(messages.Domain_Required()) // "Organization identifier is required."
                .MaximumLength(63).
                WithMessage(messages.Domain_MaxLength()) // "Organization identifier cannot exceed 63 characters."
                .Must(NotStartOrEndWithHyphen).
                WithMessage(messages.Domain_InvalidCharacters()) // "Organization identifier cannot start or end with a hyphen."
                .Matches("^[a-zA-Z0-9-]+$").
                WithMessage(messages.Domain_InvalidCharacters());// "Organization identifier can only contain letters, numbers, and hyphens."
        }

        /// <summary>
        /// Ensures the domain does not start or end with a hyphen.
        /// </summary>
        /// <param name="domain">The domain string to validate.</param>
        /// <returns>True if valid; otherwise, false.</returns>
        private static bool NotStartOrEndWithHyphen(string domain)
        {
            if (string.IsNullOrEmpty(domain))
                return false;

            return !domain.StartsWith("-") && !domain.EndsWith("-");
        }
    }
}