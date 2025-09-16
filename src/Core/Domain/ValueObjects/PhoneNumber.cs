using System.Text.RegularExpressions;
using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.ValueObjects;

// VO: شماره تلفن با اعتبارسنجی
public sealed class PhoneNumber
{
    private readonly static Regex Pattern = new("^\\+?[0-9]{7,15}$", RegexOptions.Compiled);

    public PhoneNumber(string value)
    {
        value = value?.Trim() ?? throw new DomainValidationException("شماره تلفن الزامی است.");
        if (!Pattern.IsMatch(value)) throw new DomainValidationException("فرمت شماره تلفن نامعتبر است.");

        Value = value;
    }

    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(PhoneNumber phone)
    {
        return phone.Value;
    }
}