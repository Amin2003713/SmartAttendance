namespace SmartAttendance.Application.Features.Users.Requests;

// درخواست شروع فرآیند فراموشی رمز عبور
public sealed class ForgotPasswordRequest
{
    public string EmailOrUsername { get; init; } = string.Empty;
}