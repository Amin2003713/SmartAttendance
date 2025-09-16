using SmartAttendance.Domain.Calenders.DailyCalender;

namespace SmartAttendance.Application.Interfaces.Calendars.DailyCalendars;

public interface IDailyCalendarCommandRepository : ICommandRepository<DailyCalendar>,
    IScopedDependency;