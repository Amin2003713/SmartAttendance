using SmartAttendance.Application.Features.Plans.Responses;

namespace SmartAttendance.Application.Features.Calendars.Request.Queries.GetCalendar;

public class GetCalendarResponse
{
    public DateTime Date { get; set; }
    public bool IsHoliday { get; set; }
    public List<GetPlanInfoCalendarResponse> PlanInfos { get; set; } = [] ;
    public List<string> Details { get; set; }     = [];
}