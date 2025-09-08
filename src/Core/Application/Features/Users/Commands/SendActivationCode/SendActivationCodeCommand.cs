namespace SmartAttendance.Application.Features.Users.Commands.SendActivationCode;

public class SendActivationCodeCommand : IRequest<SendActivationCodeCommandResponse>
{
    public string PhoneNumber { get; set; }
}