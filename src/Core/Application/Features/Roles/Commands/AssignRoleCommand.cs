using SmartAttendance.Application.Features.Roles.Requests;

namespace SmartAttendance.Application.Features.Roles.Commands;

// Command: انتساب نقش به کاربر
public sealed record AssignRoleCommand(
    AssignRoleRequest Request
) : IRequest;

// Handler: انتساب نقش