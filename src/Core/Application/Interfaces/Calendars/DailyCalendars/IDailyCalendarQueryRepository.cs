using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Features.Calendars.Request.Queries.GetHoliday;
using SmartAttendance.Application.Features.Calendars.Request.Queries.GetReminder;
using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Calenders.DailyCalender;

namespace SmartAttendance.Application.Interfaces.Calendars.DailyCalendars;

public interface IDailyCalendarQueryRepository : IQueryRepository<DailyCalendar>,
    IScopedDependency
{
    Task<List<DailyCalendar>?> GetCustomCalendarEvents(
        DateTime          startDate,
        DateTime          date,
        Guid              userId,
        CancellationToken cancellationToken);

    Task<bool> IsAlreadyHoliday(DateTime dateTime, CancellationToken cancellationToken);

    Task<List<GetHolidayResponse>> GetHolidaysForMonth(
        DateTime          startAt,
        DateTime          endAt,
        CancellationToken cancellationToken);

    Task<List<GetReminderResponse>> GetReminderForProject(
        Guid              userId,
        DateTime          startAt,
        DateTime          endAt,
        CancellationToken cancellationToken);

    Task<DailyCalendar?> Getday(Guid Id, CancellationToken cancellationToken);
}