using SmartAttendance.Application.Features.Users.Requests;

namespace SmartAttendance.Application.Features.Users.Commands;

public sealed record ForgotPasswordCommand(
    ForgotPasswordRequest Request
) : IRequest;