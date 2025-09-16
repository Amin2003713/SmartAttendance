using DNTPersianUtils.Core;
using SmartAttendance.Application.Features.Calendars.Queries.GetReminder;
using SmartAttendance.Application.Features.Calendars.Request.Queries.GetReminder;
using SmartAttendance.Application.Interfaces.Calendars.DailyCalendars;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Calendars.Queries.GetReminder;

public class GetReminderQueryHandler(
    IdentityService                  service,
    IDailyCalendarQueryRepository    dailyCalendarQueryRepository,
    ILogger<GetReminderQueryHandler> logger
)
    : IRequestHandler<GetReminderQuery, List<GetReminderResponse>>
{
    public async Task<List<GetReminderResponse>> Handle(GetReminderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId       = service.GetUserId<Guid>();
            var startOfMonth = new PersianDateTime(request.Year, request.Month, 1).ToString().ToGregorianDateTime();
            var range        = startOfMonth.GetPersianMonthStartAndEndDates();
            var fromDate     = startOfMonth!.Value;
            var toDate       = range!.EndDate;

            logger.LogInformation(
                "Getting reminders , UserId: {UserId}, DateRange: {From} to {To}",
                userId,
                fromDate,
                toDate);

            var reminders = await dailyCalendarQueryRepository.GetReminderForProject(
                userId,
                fromDate,
                toDate,
                cancellationToken);

            logger.LogInformation("Found {Count} reminders  UserId: {UserId}",
                reminders.Count,
                userId);

            return reminders;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error retrieving reminders  Month: {Month}, Year: {Year}",
                request.Month,
                request.Year);

            throw;
        }
    }
}