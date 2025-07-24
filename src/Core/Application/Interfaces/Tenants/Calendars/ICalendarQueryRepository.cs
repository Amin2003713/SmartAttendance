using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Calendars.Request.Queries.GetHoliday;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Tenants;

namespace Shifty.Application.Interfaces.Tenants.Calendars;

public interface ICalendarQueryRepository : IScopedDependency
{
    Task<List<TenantCalendar>> GetPublicCalendarEvents(
        Expression<Func<TenantCalendar, bool>> predicate,
        CancellationToken cancellationToken);

    Task<bool> IsAlreadyHoliday(Guid projectId, DateTime dateTime, CancellationToken cancellationToken);

    Task<List<GetHolidayResponse>> GetHolidaysForMonth(
        Guid projectId,
        DateTime startAt,
        DateTime endAt,
        CancellationToken cancellationToken);
}