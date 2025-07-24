using Shifty.Application.Calendars.Request.Commands.CreateReminder;

namespace Shifty.Application.Calendars.Request.Commands.UpdateReminder;

public class UpdateReminderRequest
{
    public Guid ReminderId { get; set; }
    public string Details { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime Date { get; set; }

    public List<UserTargetRequest> TargetUsers { get; set; } = new();
}