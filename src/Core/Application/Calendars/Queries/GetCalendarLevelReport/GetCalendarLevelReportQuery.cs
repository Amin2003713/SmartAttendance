using Shifty.Application.Calendars.Request.Queries.GetCalendarLevelReport;

namespace Shifty.Application.Calendars.Queries.GetCalendarLevelReport;

public record GetCalendarLevelReportQuery(

    DateTime Date
) : IRequest<List<CalendarInfoForLevelDto>>;