namespace SmartAttendance.Application.Features.Users.Requests;

// درخواست ویرایش پروفایل کاربر
public sealed class UpdateUserRequest
{
	public string FirstName { get; init; } = string.Empty;
	public string LastName { get; init; } = string.Empty;
	public string Email { get; init; } = string.Empty;
	public string Phone { get; init; } = string.Empty;
}

