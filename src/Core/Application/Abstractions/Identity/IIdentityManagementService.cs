namespace SmartAttendance.Application.Abstractions.Identity;

// سرویس مدیریت هویت برای فراموشی/بازنشانی رمز عبور (در زیرساخت پیاده‌سازی می‌شود)
public interface IIdentityManagementService
{
	Task StartForgotPasswordAsync(string emailOrUsername, CancellationToken ct = default);
	Task ResetPasswordAsync(string emailOrUsername, string token, string newPassword, CancellationToken ct = default);
}

