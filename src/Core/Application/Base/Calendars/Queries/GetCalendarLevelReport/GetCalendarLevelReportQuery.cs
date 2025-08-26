using Shifty.Application.Base.Calendars.Request.Queries.GetCalendarLevelReport;

namespace Shifty.Application.Base.Calendars.Queries.GetCalendarLevelReport;

public record GetCalendarLevelReportQuery(

    DateTime Date
) : IRequest<List<CalendarInfoForLevelDto>>;