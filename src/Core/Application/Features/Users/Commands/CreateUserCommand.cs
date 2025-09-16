using SmartAttendance.Application.Features.Users.Requests;
using SmartAttendance.Application.Features.Users.Responses;

namespace SmartAttendance.Application.Features.Users.Commands;

// Command: ایجاد کاربر جدید
public sealed record CreateUserCommand(
    CreateUserRequest Request
) : IRequest<UserProfileDto>;

// Handler: ایجاد کاربر جدید