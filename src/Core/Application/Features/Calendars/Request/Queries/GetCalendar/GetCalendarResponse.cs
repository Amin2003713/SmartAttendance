namespace Shifty.Application.Features.Calendars.Request.Queries.GetCalendar;

public class GetCalendarResponse
{
    public DateTime Date { get; set; }
    public bool IsHoliday { get; set; }

    public bool ActionNeeded { get; set; }

    public bool IsCustomHoliday { get; set; }

    public bool HasReminder { get; set; }

    public bool IsVerified { get; set; } = false;
}