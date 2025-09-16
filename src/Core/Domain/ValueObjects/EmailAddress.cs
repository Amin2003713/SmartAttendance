using System.Text.RegularExpressions;
using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.ValueObjects;

// VO: آدرس ایمیل با اعتبارسنجی
public sealed class EmailAddress
{
    private readonly static Regex Pattern = new("^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public EmailAddress(string value)
    {
        value = value?.Trim() ?? throw new DomainValidationException("ایمیل الزامی است.");
        if (value.Length > 254) throw new DomainValidationException("طول ایمیل نامعتبر است.");
        if (!Pattern.IsMatch(value)) throw new DomainValidationException("فرمت ایمیل نامعتبر است.");

        Value = value;
    }

    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(EmailAddress email)
    {
        return email.Value;
    }
}