using SmartAttendance.Application.Features.Calendars.Request.Queries.GetHoliday;

namespace SmartAttendance.Application.Features.Calendars.Queries.GetHoliday;

public record GetHolidayQuery(
    int Year,
    int Month
) : IRequest<List<GetHolidayResponse>>;