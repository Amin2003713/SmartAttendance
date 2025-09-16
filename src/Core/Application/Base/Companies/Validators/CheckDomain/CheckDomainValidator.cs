using SmartAttendance.Application.Base.Companies.Queries.CheckDomain;

namespace SmartAttendance.Application.Base.Companies.Validators.CheckDomain;

public class CheckDomainValidator : AbstractValidator<CheckDomainQuery>
{
    public CheckDomainValidator(IStringLocalizer<CheckDomainValidator> localizer)
    {
        RuleFor(x => x.Domain).
            Cascade(CascadeMode.Stop) // Stop validation on first failure
            .
            NotEmpty().
            WithMessage(localizer["Organization identifier is required."].Value) // "Organization identifier is required."
            .
            MaximumLength(63).
            WithMessage(localizer["Organization identifier cannot exceed 63 characters."].
                            Value) // "Organization identifier cannot exceed 63 characters."
            .
            Must(NotStartOrEndWithHyphen).
            WithMessage(localizer["Organization identifier cannot start or end with a hyphen."].
                            Value) // "Organization identifier cannot start or end with a hyphen."
            .
            Matches("^[a-zA-Z0-9-]+$").
            WithMessage(localizer["Organization identifier can only contain letters, numbers, and hyphens."].
                            Value); // "Organization identifier can only contain letters, numbers, and hyphens."
    }

    /// <summary>
    ///     Ensures the domain does not start or end with a hyphen.
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