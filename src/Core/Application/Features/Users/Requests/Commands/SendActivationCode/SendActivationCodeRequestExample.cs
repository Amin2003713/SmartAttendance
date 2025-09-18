namespace SmartAttendance.Application.Features.Users.Requests.Commands.SendActivationCode;

public class SendActivationCodeRequestExample : IExamplesProvider<SendActivationCodeRequest>
{
    public SendActivationCodeRequest GetExamples()
    {
        return new SendActivationCodeRequest
        {
            PhoneNumber = "09131283883"
        };
    }
}