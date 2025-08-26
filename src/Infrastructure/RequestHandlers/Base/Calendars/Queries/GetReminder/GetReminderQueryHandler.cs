using DNTPersianUtils.Core;
using Shifty.Application.Base.Calendars.Queries.GetReminder;
using Shifty.Application.Base.Calendars.Request.Queries.GetReminder;
using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Base.Calendars.Queries.GetReminder;

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
            var fromDate     = startOfMonth!.Value;
            var toDate       = range!.EndDate;

            logger.LogInformation(
                "Getting reminders for ProjectId: UserId: {UserId}, DateRange: {From} to {To}",
                userId,
                fromDate,
                toDate);

            var reminders = await dailyCalendarQueryRepository.GetReminderForProject(
        
                userId,
                fromDate,
                toDate,
                cancellationToken);

            logger.LogInformation("Found {Count} reminders for ProjectId: {ProjectId}, UserId: {UserId}",
                reminders.Count,
             
                userId);

            return reminders;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error retrieving reminders for ProjectId: {ProjectId}, Month: {Month}, Year: {Year}",
           
                request.Month,
                request.Year);

            throw;
        }
    }
}