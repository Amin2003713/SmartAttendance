using Shifty.Application.Calendars.Request.Queries.GetReminder;

namespace Shifty.Application.Calendars.Queries.GetReminder;

public record GetReminderQuery(
    Guid ProjectId,
    int Year,
    int Month
) : IRequest<List<GetReminderResponse>>;