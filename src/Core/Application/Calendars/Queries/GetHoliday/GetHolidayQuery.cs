using Shifty.Application.Calendars.Request.Queries.GetHoliday;

namespace Shifty.Application.Calendars.Queries.GetHoliday;

public record GetHolidayQuery(
    
    int year,
    int month
) : IRequest<List<GetHolidayResponse>>;