using SmartAttendance.Application.Features.Roles.Requests;

namespace SmartAttendance.Application.Features.Roles.Commands;

// Command: ایجاد نقش جدید
public sealed record CreateRoleCommand(
    CreateRoleRequest Request
) : IRequest;