using Shifty.Application.Calendars.Queries.GetReminder;
using Shifty.Application.Calendars.Request.Queries.GetReminder;
using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Calendars.Queries.GetReminder;

public class GetReminderQueryHandler(
    IdentityService service,
    IDailyCalendarQueryRepository dailyCalendarQueryRepository,
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
            var fromDate     = startOfMonth.Value;
            var toDate       = range!.EndDate;

            logger.LogInformation(
                "Getting reminders for ProjectId: {ProjectId}, UserId: {UserId}, DateRange: {From} to {To}",
                request.ProjectId,
                userId,
                fromDate,
                toDate);

            var reminders = await dailyCalendarQueryRepository.GetReminderForProject(
                request.ProjectId,
                userId,
                fromDate,
                toDate,
                cancellationToken);

            logger.LogInformation("Found {Count} reminders for ProjectId: {ProjectId}, UserId: {UserId}",
                reminders.Count,
                request.ProjectId,
                userId);

            return reminders;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error retrieving reminders for ProjectId: {ProjectId}, Month: {Month}, Year: {Year}",
                request.ProjectId,
                request.Month,
                request.Year);

            throw;
        }
    }
}