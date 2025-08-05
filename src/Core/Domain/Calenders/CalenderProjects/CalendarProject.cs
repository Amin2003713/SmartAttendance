using Shifty.Common.General.BaseClasses;

namespace Shifty.Domain.Calenders.CalenderProjects;

public class CalendarProject : BaseEntity
{


    public DailyCalendar Calendar { get; set; }
    public Guid? CalendarId { get; set; }
}