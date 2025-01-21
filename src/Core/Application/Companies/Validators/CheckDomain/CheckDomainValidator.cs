using FluentValidation;
using Shifty.Application.Companies.Queries.CheckDomain;

namespace Shifty.Application.Companies.Validators.CheckDomain
{
    public class CheckDomainValidator  : AbstractValidator<CheckDomainQuery>
    {
        public CheckDomainValidator()
        {
            RuleFor(x => x.Domain).
                Cascade(CascadeMode.Stop) // Stop validation on first failure
                .NotEmpty().
                WithMessage("شناسه سازمان الزامی است.") // "Organization identifier is required."
                .MaximumLength(63).
                WithMessage("شناسه سازمان نمی‌تواند بیشتر از 63 کاراکتر باشد.") // "Organization identifier cannot exceed 63 characters."
                .Must(NotStartOrEndWithHyphen).
                WithMessage(
                    "شناسه سازمان نمی‌تواند با خط فاصله شروع یا پایان یابد.") // "Organization identifier cannot start or end with a hyphen."
                .Matches("^[a-zA-Z0-9-]+$").
                WithMessage(
                    "شناسه سازمان فقط می‌تواند شامل حروف الفبا، اعداد، و خط فاصله باشد."); // "Organization identifier can only contain letters, numbers, and hyphens."
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