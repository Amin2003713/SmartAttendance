using SmartAttendance.Application.Features.Users.Requests;

namespace SmartAttendance.Application.Features.Users.Commands;

public sealed record ResetPasswordCommand(
    ResetPasswordRequest Request
) : IRequest;