using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Calenders.CalenderUsers;

namespace Shifty.Application.Interfaces.Calendars.CalendarUsers;

public interface ICalendarUserQueryRepository : IQueryRepository<CalendarUser>,
    IScopedDependency
{
    Task<List<CalendarUser>> GetCalendarUserByCalendarId(Guid calendarId, CancellationToken cancellationToken);
}