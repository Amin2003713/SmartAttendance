namespace SmartAttendance.Application.Features.Roles.Requests;

// درخواست انتساب نقش به کاربر
public sealed class AssignRoleRequest
{
    public Guid UserId { get; init; }
    public string RoleName { get; init; } = string.Empty;
}