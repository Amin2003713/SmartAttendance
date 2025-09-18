namespace SmartAttendance.Application.Features.Users.Requests.Commands.Verify;

public class VerifyPhoneNumberRequestExample : IExamplesProvider<VerifyPhoneNumberRequest>
{
    public VerifyPhoneNumberRequest GetExamples()
    {
        return new VerifyPhoneNumberRequest
        {
            PhoneNumber = "09131283883", // Example phone number
            Code        = "123456"       // Example verification code
        };
    }
}