using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Calenders.CalenderUsers;

namespace SmartAttendance.Application.Interfaces.Calendars.CalendarUsers;

public interface ICalendarUserQueryRepository : IQueryRepository<CalendarUser>,
    IScopedDependency
{
    Task<List<CalendarUser>> GetCalendarUserByCalendarId(Guid calendarId, CancellationToken cancellationToken);
}