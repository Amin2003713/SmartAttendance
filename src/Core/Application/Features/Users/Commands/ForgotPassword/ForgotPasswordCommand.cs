using Shifty.Application.Features.Users.Requests.Commands.ForgotPassword;

namespace Shifty.Application.Features.Users.Commands.ForgotPassword;

/// <summary>
///     Represents a command to reset a user's password. Inherits from <see cref="ForgotPasswordRequest" />.
/// </summary>
public class ForgotPasswordCommand : ForgotPasswordRequest,
    IRequest;