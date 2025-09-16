using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.ValueObjects;

// VO: توکن QR با انقضا
public sealed class QRToken
{
	public string Token { get; }
	public DateTime ExpiresAtUtc { get; }

	public QRToken(string token, DateTime expiresAtUtc)
	{
		token = token?.Trim() ?? throw new DomainValidationException("توکن QR الزامی است.");
		if (token.Length < 8) throw new DomainValidationException("توکن QR بسیار کوتاه است.");
		if (expiresAtUtc <= DateTime.UtcNow) throw new DomainValidationException("توکن QR منقضی شده است.");
		Token = token;
		ExpiresAtUtc = expiresAtUtc;
	}

	public bool IsExpired() => DateTime.UtcNow > ExpiresAtUtc;

	public override string ToString() => Token;
}

