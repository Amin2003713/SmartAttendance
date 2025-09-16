using System.Text.RegularExpressions;
using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.ValueObjects;

// VO: آدرس ایمیل با اعتبارسنجی
public sealed class EmailAddress
{
	private static readonly Regex Pattern = new("^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	public string Value { get; }

	public EmailAddress(string value)
	{
		value = value?.Trim() ?? throw new DomainValidationException("ایمیل الزامی است.");
		if (value.Length > 254) throw new DomainValidationException("طول ایمیل نامعتبر است.");
		if (!Pattern.IsMatch(value)) throw new DomainValidationException("فرمت ایمیل نامعتبر است.");
		Value = value;
	}

	public override string ToString() => Value;

	public static implicit operator string(EmailAddress email) => email.Value;
}

