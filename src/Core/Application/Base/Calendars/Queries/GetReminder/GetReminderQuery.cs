using Shifty.Application.Base.Calendars.Request.Queries.GetReminder;

namespace Shifty.Application.Base.Calendars.Queries.GetReminder;

public record GetReminderQuery(

    int Year,
    int Month
) : IRequest<List<GetReminderResponse>>;