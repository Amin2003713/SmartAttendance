using SmartAttendance.Application.Features.Calendars.Request.Queries.GetCalendar;

namespace SmartAttendance.Application.Features.Calendars.Queries.GetCalendar;

public record GetCalendarQuery(
    int Year,
    int Month
) : IRequest<List<GetCalendarResponse>>;