using MediatR;

namespace Shifty.Application.Users.Command.SendActivationCode
{
    public class SendActivationCodeCommand : IRequest<SendActivationCodeResponse>
    {
        public string PhoneNumber { get; set; }
    }
}