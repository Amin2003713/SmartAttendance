using System;

namespace Shifty.Application.Users.Queries.SendActivationCode
{
    public class SendActivationCodeQueryResponse
    {
        public TimeSpan SentDateTime { get; set; } = TimeSpan.FromSeconds(120);
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}