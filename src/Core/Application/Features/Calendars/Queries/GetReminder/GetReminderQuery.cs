using Shifty.Application.Features.Calendars.Request.Queries.GetReminder;

namespace Shifty.Application.Features.Calendars.Queries.GetReminder;

public record GetReminderQuery(
    int Year,
    int Month
) : IRequest<List<GetReminderResponse>>;