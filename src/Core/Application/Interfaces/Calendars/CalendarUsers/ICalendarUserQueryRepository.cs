using SmartAttendance.Domain.Calenders.CalenderUsers;

namespace SmartAttendance.Application.Interfaces.Calendars.CalendarUsers;

public interface ICalendarUserQueryRepository : IQueryRepository<CalendarUser>,
    IScopedDependency
{
    Task<List<CalendarUser>> GetCalendarUserByCalendarId(Guid calendarId, CancellationToken cancellationToken);
}