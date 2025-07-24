namespace Shifty.Domain.Calenders.CalenderProjects;

public class CalendarProject : BaseEntity
{
    public Guid ProjectId { get; set; }

    public DailyCalendar Calendar { get; set; }
    public Guid? CalendarId { get; set; }
}