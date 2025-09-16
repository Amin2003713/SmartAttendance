namespace SmartAttendance.Application.Features.Users.Responses;

// DTO اطلاعات پروفایل کاربر
public sealed class UserProfileDto
{
	public Guid Id { get; init; }
	public string FullName { get; init; } = string.Empty;
	public string Email { get; init; } = string.Empty;
	public string Phone { get; init; } = string.Empty;
	public string NationalCode { get; init; } = string.Empty;
	public bool IsLocked { get; init; }
}

