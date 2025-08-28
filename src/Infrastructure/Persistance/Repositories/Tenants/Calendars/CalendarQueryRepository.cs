using Shifty.Application.Base.Calendars.Request.Queries.GetHoliday;

namespace Shifty.Persistence.Repositories.Tenants.Calendars;

public class CalendarQueryRepository(
    ShiftyTenantDbContext db,
    ILogger<CalendarQueryRepository> logger,
    IStringLocalizer<CalendarQueryRepository> localizer
)
    : ICalendarQueryRepository
{
    public async Task<List<TenantCalendar>> GetPublicCalendarEvents(
        Expression<Func<TenantCalendar, bool>> predicate,
        CancellationToken cancellationToken)
    {
        try
        {
            return await db.TenantCalendars.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching public calendar event for date: ");

            throw new InvalidOperationException(
                localizer["An error occurred while retrieving public calendar event."]);
        }
    }

    public async Task<bool> IsAlreadyHoliday( DateTime dateTime, CancellationToken cancellationToken)
    {
        try
        {
            // logger.LogInformation("Checking if date {Date} is already a holiday for project {ProjectId}",
            //     dateTime,
            //     projectId);

            var result = await db.TenantCalendars.AnyAsync(
                c => c.Date == dateTime && (c.IsHoliday || c.IsWeekend),
                cancellationToken
            );

            return result;
        }
        catch (Exception ex)
        {
            // logger.LogError(ex,
            //     "Error checking existing holiday for projectId: {ProjectId}, date: {Date}",
            //     projectId,
            //     dateTime);

            throw new InvalidOperationException(localizer["An error occurred while checking holiday status."]);
        }
    }

    public async Task<List<GetHolidayResponse>> GetHolidaysForMonth(
        DateTime startAt,
        DateTime endAt,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching holidays from {Start} to {End} ",
                startAt,
                endAt);

            var publicHolidays = await db.TenantCalendars
                .Where(e => e.Date >= startAt && e.Date <= endAt && e.IsHoliday)
                .Select(e => new GetHolidayResponse
                {
                    Date = e.Date,
                    Message = e.Details,
                    IsCustom = false,
                    Id = e.Id
                })
                .ToListAsync(cancellationToken);

            return publicHolidays.OrderBy(h => h.Date).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching holidays for month range: {Start} - {End}", startAt, endAt);
            throw new InvalidOperationException(localizer["An error occurred while retrieving holidays."]);
        }
    }


    public async Task<TenantCalendar> Getday(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching calendar day by Id: {Id}", id);
            var reminder = await db.TenantCalendars.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (reminder == null)
            {
                logger.LogWarning("Calendar day not found for Id: {Id}", id);
                throw new KeyNotFoundException(localizer["Day not found for the given Id."]);
            }

            return reminder;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving calendar day for Id: {Id}", id);
            throw new InvalidOperationException(localizer["An error occurred while retrieving calendar day."]);
        }
    }
}