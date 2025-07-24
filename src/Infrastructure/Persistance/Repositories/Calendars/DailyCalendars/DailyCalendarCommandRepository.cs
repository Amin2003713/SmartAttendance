using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Domain.Calenders.DailyCalender;

namespace Shifty.Persistence.Repositories.Calendars.DailyCalendars;

public class DailyCalendarCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<DailyCalendar>> logger
)
    : CommandRepository<DailyCalendar>(dbContext, logger),
        IDailyCalendarCommandRepository { }