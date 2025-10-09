namespace SmartAttendance.Persistence.Repositories.Tenants.Calendars;

public class CalendarQueryRepository(
    SmartAttendanceTenantDbContext            db,
    ILogger<CalendarQueryRepository>          logger,
    IStringLocalizer<CalendarQueryRepository> localizer
)
    : ICalendarQueryRepository
{
    public async Task<Dictionary<DateTime, List<TenantCalendar>>> GetPublicCalendarEvents(
        Expression<Func<TenantCalendar, bool>> predicate,
        CancellationToken cancellationToken)
    {
        try
        {
            return await db.TenantCalendars.AsNoTracking()
                .Where(predicate)
                .GroupBy(a => a.Date.Date)
                .ToDictionaryAsync( key => key.Key , values =>  values?.ToList() ?? [] , cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching public calendar event for date: ");

            throw new InvalidOperationException(
                localizer["An error occurred while retrieving public calendar event."]);
        }
    }

    public async Task<bool> IsAlreadyHoliday(DateTime dateTime, CancellationToken cancellationToken)
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


    public async Task<TenantCalendar> GetDay(Guid id, CancellationToken cancellationToken)
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