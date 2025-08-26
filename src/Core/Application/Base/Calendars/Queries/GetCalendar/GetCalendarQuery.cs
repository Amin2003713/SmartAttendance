using Shifty.Application.Base.Calendars.Request.Queries.GetCalendar;

namespace Shifty.Application.Base.Calendars.Queries.GetCalendar;

public record GetCalendarQuery(

    int Year,
    int Month
) : IRequest<List<GetCalendarResponse>>;