using MediatR;
using System;

namespace Shifty.Application.Users.Command.SendActivationCode
{
    public class SendActivationCodeResponse
    {
        public DateTime SentDateTime { get; set; }
        public bool WasSuccess { get; set; }
    }

    public class SendActivationCodeCommand : IRequest<SendActivationCodeResponse>
    {
        public string UserId { get; set; }
    }
}