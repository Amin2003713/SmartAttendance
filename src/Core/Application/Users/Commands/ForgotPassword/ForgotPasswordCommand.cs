using Shifty.Application.Users.Requests.Commands.ForgotPassword;

namespace Shifty.Application.Users.Commands.ForgotPassword;

/// <summary>
///     Represents a command to reset a user's password. Inherits from <see cref="ForgotPasswordRequest" />.
/// </summary>
public class ForgotPasswordCommand : ForgotPasswordRequest,
    IRequest;