using SmartAttendance.Application.Features.Users.Requests.Commands.ForgotPassword;

namespace SmartAttendance.Application.Features.Users.Commands.ForgotPassword;

/// <summary>
///     Represents a command to reset a user's password. Inherits from <see cref="ForgotPasswordRequest" />.
/// </summary>
public class ForgotPasswordCommand : ForgotPasswordRequest,
    IRequest;