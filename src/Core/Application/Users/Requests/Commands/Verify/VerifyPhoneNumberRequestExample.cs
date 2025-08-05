namespace Shifty.Application.Users.Requests.Commands.Verify;

public class VerifyPhoneNumberRequestExample : IExamplesProvider<VerifyPhoneNumberRequest>
{
    public VerifyPhoneNumberRequest GetExamples()
    {
        return new VerifyPhoneNumberRequest
        {
            PhoneNumber = "09134041709", // Example phone number
            Code = "123456"              // Example verification code
        };
    }
}