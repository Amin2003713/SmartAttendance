using Shifty.Application.Features.Calendars.Request.Queries.GetCalendarLevelReport;

namespace Shifty.Application.Features.Calendars.Queries.GetCalendarLevelReport;

public record GetCalendarLevelReportQuery(
    DateTime Date
) : IRequest<List<CalendarInfoForLevelResponse>>;