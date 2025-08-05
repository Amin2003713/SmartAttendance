using Shifty.Application.Calendars.Request.Queries.GetReminder;

namespace Shifty.Application.Calendars.Queries.GetReminder;

public record GetReminderQuery(

    int Year,
    int Month
) : IRequest<List<GetReminderResponse>>;