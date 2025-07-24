using Shifty.Application.Calendars.Request.Queries.GetCalendarLevelReport;

namespace Shifty.Application.Calendars.Queries.GetCalendarLevelReport;

public record GetCalendarLevelReportQuery(
    Guid ProjectId,
    DateTime Date
) : IRequest<List<CalendarInfoForLevelDto>>;