namespace SmartAttendance.Application.Features.Users.Requests;

// درخواست ایجاد کاربر جدید
public sealed class CreateUserRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string NationalCode { get; init; } = string.Empty;
}