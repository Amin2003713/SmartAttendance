namespace Shifty.Application.Users.Commands.SendActivationCode;

public class SendActivationCodeCommand : IRequest<SendActivationCodeCommandResponse>
{
    public string PhoneNumber { get; set; }
}