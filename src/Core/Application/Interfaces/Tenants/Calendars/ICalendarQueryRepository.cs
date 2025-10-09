using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Tenants;

namespace SmartAttendance.Application.Interfaces.Tenants.Calendars;

public interface ICalendarQueryRepository : IScopedDependency
{
    Task<List<TenantCalendar>> GetPublicCalendarEvents(
        Expression<Func<TenantCalendar, bool>> predicate,
        CancellationToken                      cancellationToken);

    Task<bool> IsAlreadyHoliday(DateTime dateTime, CancellationToken cancellationToken);

}