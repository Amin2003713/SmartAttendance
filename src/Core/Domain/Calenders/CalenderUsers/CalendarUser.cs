using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Calenders.CalenderUsers;

public class CalendarUser : BaseEntity
{
    public Guid UserId { get; set; }

    public DailyCalendar Calendar { get; set; }
    public Guid? CalendarId { get; set; }
}