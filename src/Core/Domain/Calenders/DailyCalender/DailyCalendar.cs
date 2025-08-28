using Shifty.Common.General.BaseClasses;

namespace Shifty.Domain.Calenders.DailyCalender;

public class DailyCalendar : BaseEntity
{
    public DateTime Date { get; set; }
    public bool IsHoliday { get; set; } = false;
    public bool IsMeeting { get; set; } = false;
    public bool IsReminder { get; set; }
    public string? Details { get; set; } = null!;
    public ICollection<CalendarUser> CalendarUsers { get; set; }

    public void SetReminder()
    {
        IsReminder = true;
    }
}