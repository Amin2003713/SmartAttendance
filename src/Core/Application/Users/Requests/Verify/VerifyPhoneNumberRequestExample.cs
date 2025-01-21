using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Users.Requests.Verify
{
    public class VerifyPhoneNumberRequestExample : IExamplesProvider<VerifyPhoneNumberRequest>
    {
        public VerifyPhoneNumberRequest GetExamples()
        {
            return new VerifyPhoneNumberRequest
            {
                PhoneNumber = "09134041709s" , // Example phone number
                Code        = "123456" ,       // Example verification code
            };
        }
    }
}