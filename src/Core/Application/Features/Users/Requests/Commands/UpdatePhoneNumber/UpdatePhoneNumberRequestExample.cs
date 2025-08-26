namespace Shifty.Application.Features.Users.Requests.Commands.UpdatePhoneNumber;

public class UpdatePhoneNumberRequestExample : IExamplesProvider<UpdatePhoneNumberRequest>
{
    public UpdatePhoneNumberRequest GetExamples()
    {
        return new UpdatePhoneNumberRequest
        {
            PhoneNumber = "09121234567"
        };
    }
}