using Shifty.Application.Features.Calendars.Request.Queries.GetCalendar;

namespace Shifty.Application.Features.Calendars.Queries.GetCalendar;

public record GetCalendarQuery(
    int Year,
    int Month
) : IRequest<List<GetCalendarResponse>>;