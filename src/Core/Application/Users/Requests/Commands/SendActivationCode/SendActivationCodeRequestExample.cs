using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Users.Requests.Commands.SendActivationCode;

public class SendActivationCodeRequestExample : IExamplesProvider<SendActivationCodeRequest>
{
    public SendActivationCodeRequest GetExamples()
    {
        return new SendActivationCodeRequest
        {
            PhoneNumber = "09134041709"
        };
    }
}