using Shifty.Application.Features.Calendars.Request.Queries.GetHoliday;

namespace Shifty.Application.Features.Calendars.Queries.GetHoliday;

public record GetHolidayQuery(
    int Year,
    int Month
) : IRequest<List<GetHolidayResponse>>;