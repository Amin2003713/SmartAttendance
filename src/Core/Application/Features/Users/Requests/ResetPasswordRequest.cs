namespace SmartAttendance.Application.Features.Users.Requests;

// درخواست بازنشانی رمز عبور با توکن
public sealed class ResetPasswordRequest
{
	public string EmailOrUsername { get; init; } = string.Empty;
	public string Token { get; init; } = string.Empty;
	public string NewPassword { get; init; } = string.Empty;
}

