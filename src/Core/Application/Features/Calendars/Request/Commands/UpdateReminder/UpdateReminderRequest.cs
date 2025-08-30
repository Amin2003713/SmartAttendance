using Shifty.Application.Features.Calendars.Request.Commands.CreateReminder;

namespace Shifty.Application.Features.Calendars.Request.Commands.UpdateReminder;

public class UpdateReminderRequest
{
    public Guid ReminderId { get; set; }
    public string Details { get; set; }
    public DateTime Date { get; set; }

    public List<UserTargetRequest> TargetUsers { get; set; } = [];
}