using System.Linq.Expressions;
using SmartAttendance.Application.Features.Calendars.Request.Queries.GetHoliday;

namespace SmartAttendance.Application.Interfaces.Tenants.Calendars;

public interface ICalendarQueryRepository : IScopedDependency
{
    Task<List<TenantCalendar>> GetPublicCalendarEvents(
        Expression<Func<TenantCalendar, bool>> predicate,
        CancellationToken                      cancellationToken);

    Task<bool> IsAlreadyHoliday(DateTime dateTime, CancellationToken cancellationToken);

    Task<List<GetHolidayResponse>> GetHolidaysForMonth(
        DateTime          startAt,
        DateTime          endAt,
        CancellationToken cancellationToken);
}