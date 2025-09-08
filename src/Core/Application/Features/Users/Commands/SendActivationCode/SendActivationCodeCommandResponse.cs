namespace SmartAttendance.Application.Features.Users.Commands.SendActivationCode;

public class SendActivationCodeCommandResponse
{
    public TimeSpan SentDateTime { get; set; } = TimeSpan.FromSeconds(120);
    public bool Success { get; set; }
    public string Message { get; set; }
}