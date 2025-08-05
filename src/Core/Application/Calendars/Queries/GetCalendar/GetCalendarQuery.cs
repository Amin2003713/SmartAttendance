using Shifty.Application.Calendars.Request.Queries.GetCalendar;

namespace Shifty.Application.Calendars.Queries.GetCalendar;

public record GetCalendarQuery(

    int Year,
    int Month
) : IRequest<List<GetCalendarResponse>>;