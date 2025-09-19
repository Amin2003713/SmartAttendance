using FluentValidation;
using SmartAttendance.Application.Base.Universities.Queries.CheckDomain;

namespace SmartAttendance.Application.Base.Universities.Validators.CheckDomain;

public class CheckDomainValidator : AbstractValidator<CheckDomainQuery>
{
    public CheckDomainValidator()
    {
        RuleFor(x => x.Domain)
            .Cascade(CascadeMode.Stop) // Stop validation on first failure
            .NotEmpty()
            .WithMessage("شناسه سازمان الزامی است.")
            .MaximumLength(63)
            .WithMessage("شناسه سازمان نباید بیشتر از ۶۳ کاراکتر باشد.")
            .Must(NotStartOrEndWithHyphen)
            .WithMessage("شناسه سازمان نباید با خط تیره شروع یا پایان یابد.")
            .Matches("^[a-zA-Z0-9-]+$")
            .WithMessage("شناسه سازمان فقط می‌تواند شامل حروف، اعداد و خط تیره باشد.");
    }

    /// <summary>
    ///     بررسی می‌کند که دامنه با خط تیره شروع یا پایان نیابد.
    /// </summary>
    private static bool NotStartOrEndWithHyphen(string domain)
    {
        if (string.IsNullOrEmpty(domain))
            return false;

        return !domain.StartsWith("-") && !domain.EndsWith("-");
    }
}