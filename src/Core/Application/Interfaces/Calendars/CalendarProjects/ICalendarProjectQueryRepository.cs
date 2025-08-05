using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Calenders.CalenderProjects;

namespace Shifty.Application.Interfaces.Calendars.CalendarProjects;

public interface ICalendarProjectQueryRepository : IQueryRepository<CalendarProject>,
    IScopedDependency
{
    Task<CalendarProject> GetCalendarProjectByCalendarId(Guid calendarId, CancellationToken cancellationToken);

    Task<List<CalendarProject>>
        GetListCalendarProjectByCalendarId(Guid calendarId, CancellationToken cancellationToken);
}