namespace Shifty.Application.Features.Calendars.Request.Commands.CreateReminder;

public class CreateReminderRequest
{
    public string Details { get; set; }
    public DateTime Date { get; set; }

    public List<UserTargetRequest> TargetUsers { get; set; } = new();
}