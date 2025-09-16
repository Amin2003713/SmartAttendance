using System.Text.RegularExpressions;
using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.ValueObjects;

// VO: شماره تلفن با اعتبارسنجی
public sealed class PhoneNumber
{
	private static readonly Regex Pattern = new("^\\+?[0-9]{7,15}$", RegexOptions.Compiled);

	public string Value { get; }

	public PhoneNumber(string value)
	{
		value = value?.Trim() ?? throw new DomainValidationException("شماره تلفن الزامی است.");
		if (!Pattern.IsMatch(value)) throw new DomainValidationException("فرمت شماره تلفن نامعتبر است.");
		Value = value;
	}

	public override string ToString() => Value;

	public static implicit operator string(PhoneNumber phone) => phone.Value;
}

