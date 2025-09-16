namespace SmartAttendance.Application.Features.Roles.Requests;

// درخواست ایجاد نقش جدید
public sealed class CreateRoleRequest
{
	public string Name { get; init; } = string.Empty;
}

