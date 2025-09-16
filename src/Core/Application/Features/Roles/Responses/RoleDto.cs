namespace SmartAttendance.Application.Features.Roles.Responses;

// DTO نمایش نقش
public sealed class RoleDto
{
	public Guid Id { get; init; }
	public string Name { get; init; } = string.Empty;
}

