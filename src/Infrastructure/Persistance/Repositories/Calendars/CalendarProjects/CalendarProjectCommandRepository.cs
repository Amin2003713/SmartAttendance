namespace Shifty.Persistence.Repositories.Calendars.CalendarProjects;

public class CalendarProjectCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<CalendarProject>> logger
)
    : CommandRepository<CalendarProject>(dbContext, logger),
        ICalendarProjectCommandRepository { }