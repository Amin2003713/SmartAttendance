using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.ValueObjects;

// VO: کد ملی ایران با اعتبارسنجی رقم کنترلی
public sealed class NationalCode
{
	public string Value { get; }

	public NationalCode(string value)
	{
		value = value?.Trim() ?? throw new DomainValidationException("کد ملی الزامی است.");
		if (value.Length != 10 || !value.All(char.IsDigit))
			throw new DomainValidationException("فرمت کد ملی نامعتبر است.");

		if (!IsValidChecksum(value))
			throw new DomainValidationException("کد ملی نامعتبر است.");

		Value = value;
	}

	private static bool IsValidChecksum(string code)
	{
		var check = code[9] - '0';
		var sum = 0;
		for (int i = 0; i < 9; i++)
			sum += (code[i] - '0') * (10 - i);

		var r = sum % 11;
		return (r < 2 && check == r) || (r >= 2 && check == 11 - r);
	}

	public override string ToString() => Value;
	public static implicit operator string(NationalCode nc) => nc.Value;
}

