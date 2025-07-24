namespace Shifty.Persistence.Repositories.Calendars.CalendarUsers;

public class CalendarUserQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<CalendarUserQueryRepository> logger,
    IStringLocalizer<CalendarUserQueryRepository> localizer
)
    : QueryRepository<CalendarUser>(dbContext, logger),
        ICalendarUserQueryRepository
{
    public async Task<List<CalendarUser>> GetCalendarUserByCalendarId(
        Guid calendarId,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Retrieving calendar users by CalendarId: {CalendarId}", calendarId);

            var associatedUsers = await TableNoTracking.Where(cu => cu.CalendarId == calendarId)
                .ToListAsync(cancellationToken);

            logger.LogInformation("Retrieved {Count} calendar users for CalendarId: {CalendarId}",
                associatedUsers.Count,
                calendarId);

            return associatedUsers;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving calendar users for CalendarId: {CalendarId}", calendarId);
            throw IpaException.InternalServerError(
                localizer["An unexpected error occurred while retrieving calendar users."]);
        }
    }
}