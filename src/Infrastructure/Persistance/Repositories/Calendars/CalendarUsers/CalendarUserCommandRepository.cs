namespace Shifty.Persistence.Repositories.Calendars.CalendarUsers;

public class CalendarUserCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<CalendarUser>> logger
)
    : CommandRepository<CalendarUser>(dbContext, logger),
        ICalendarUserCommandRepository { }