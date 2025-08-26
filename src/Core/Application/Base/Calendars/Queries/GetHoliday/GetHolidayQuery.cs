using Shifty.Application.Base.Calendars.Request.Queries.GetHoliday;

namespace Shifty.Application.Base.Calendars.Queries.GetHoliday;

public record GetHolidayQuery(
    
    int year,
    int month
) : IRequest<List<GetHolidayResponse>>;