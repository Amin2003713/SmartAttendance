namespace Shifty.Application.Users.Requests.Commands.ForgotPassword;

/// <summary>
/// A request containing information required to reset a user's password.
/// </summary>
public class ForgotPasswordRequest
{
    /// <summary>
    /// The phone number associated with the user account.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// The new password to be set for the user account.
    /// </summary>
    public string NewPassword { get; set; }

    /// <summary>
    /// A confirmation of the new password (must match <see cref="NewPassword"/>).
    /// </summary>
    public string ConfirmPassword { get; set; }
}