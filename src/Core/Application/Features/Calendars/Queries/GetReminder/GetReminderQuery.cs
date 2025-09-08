using SmartAttendance.Application.Features.Calendars.Request.Queries.GetReminder;

namespace SmartAttendance.Application.Features.Calendars.Queries.GetReminder;

public record GetReminderQuery(
    int Year,
    int Month
) : IRequest<List<GetReminderResponse>>;