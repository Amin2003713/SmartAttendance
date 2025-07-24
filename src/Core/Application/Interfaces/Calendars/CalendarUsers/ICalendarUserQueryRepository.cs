using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Commons.Base;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Calenders.CalenderUsers;

namespace Shifty.Application.Interfaces.Calendars.CalendarUsers;

public interface ICalendarUserQueryRepository : IQueryRepository<CalendarUser>,
    IScopedDependency
{
    Task<List<CalendarUser>> GetCalendarUserByCalendarId(Guid calendarId, CancellationToken cancellationToken);
}