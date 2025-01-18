using System;

namespace Shifty.Application.Users.Command.SendActivationCode
{
    public class SendActivationCodeResponse
    {
        public TimeSpan SentDateTime { get; set; } = TimeSpan.FromSeconds(120);
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}