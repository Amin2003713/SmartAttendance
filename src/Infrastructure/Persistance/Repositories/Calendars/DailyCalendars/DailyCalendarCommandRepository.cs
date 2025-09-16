using SmartAttendance.Application.Interfaces.Calendars.DailyCalendars;
using SmartAttendance.Domain.Calenders.DailyCalender;

namespace SmartAttendance.Persistence.Repositories.Calendars.DailyCalendars;

public class DailyCalendarCommandRepository(
    WriteOnlyDbContext                        dbContext,
    ILogger<CommandRepository<DailyCalendar>> logger
)
    : CommandRepository<DailyCalendar>(dbContext, logger),
        IDailyCalendarCommandRepository { }