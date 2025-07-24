namespace Shifty.Persistence.Repositories.Calendars.CalendarProjects;

public class CalendarProjectQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<CalendarProjectQueryRepository> logger,
    IStringLocalizer<CalendarProjectQueryRepository> localizer
)
    : QueryRepository<CalendarProject>(dbContext, logger),
        ICalendarProjectQueryRepository
{
    public async Task<CalendarProject> GetCalendarProjectByCalendarId(
        Guid calendarId,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Retrieving calendar project by CalendarId: {CalendarId}", calendarId);

            var calendarProject =
                await TableNoTracking.FirstOrDefaultAsync(a => a.CalendarId == calendarId, cancellationToken);

            if (calendarProject == null)
            {
                logger.LogWarning("No CalendarProject found for CalendarId: {CalendarId}", calendarId);
                throw IpaException.NotFound(additionalData: localizer["Calendar project not found."]);
            }

            logger.LogInformation("Successfully retrieved calendar project for CalendarId: {CalendarId}", calendarId);
            return calendarProject;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving calendar project for CalendarId: {CalendarId}", calendarId);
            throw IpaException.InternalServerError(localizer["An unexpected error occurred. Please try again later."]);
        }
    }

    public async Task<List<CalendarProject>> GetListCalendarProjectByCalendarId(
        Guid calendarId,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Retrieving list of calendar projects by CalendarId: {CalendarId}", calendarId);

            var associatedProjects = await TableNoTracking.Where(cp => cp.CalendarId == calendarId)
                .ToListAsync(cancellationToken);

            logger.LogInformation("Retrieved {Count} calendar projects for CalendarId: {CalendarId}",
                associatedProjects.Count,
                calendarId);

            return associatedProjects;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving list of calendar projects for CalendarId: {CalendarId}", calendarId);
            throw IpaException.InternalServerError(localizer["An unexpected error occurred. Please try again later."]);
        }
    }
}