namespace SmartAttendance.Application.Features.Users.Requests.Commands.ForgotPassword;

/// <summary>
///     Provides an example for <see cref="ForgotPasswordRequest" /> to be used in API documentation.
/// </summary>
public class ForgotPasswordRequestExample : IExamplesProvider<ForgotPasswordRequest>
{
    /// <inheritdoc />
    public ForgotPasswordRequest GetExamples()
    {
        return new ForgotPasswordRequest
        {
            UserName        = "09131283883",
            NewPassword     = "@Aa123456",
            ConfirmPassword = "@Aa123456"
        };
    }
}