using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Calendars.Request.Queries.GetHoliday;
using Shifty.Application.Calendars.Request.Queries.GetReminder;
using Shifty.Application.Commons.Base;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Calenders.DailyCalender;

namespace Shifty.Application.Interfaces.Calendars.DailyCalendars;

public interface IDailyCalendarQueryRepository : IQueryRepository<DailyCalendar>,
    IScopedDependency
{
    Task<List<DailyCalendar>?> GetCustomCalendarEvents(
        DateTime startDate,
        DateTime date,
        Guid projectId,
        Guid userId,
        CancellationToken cancellationToken);

    Task<bool> IsAlreadyHoliday(Guid projectId, DateTime dateTime, CancellationToken cancellationToken);

    Task<List<GetHolidayResponse>> GetHolidaysForMonth(
        Guid projectId,
        DateTime startAt,
        DateTime endAt,
        CancellationToken cancellationToken);

    Task<List<GetReminderResponse>> GetReminderForProject(
        Guid projectId,
        Guid userId,
        DateTime startAt,
        DateTime endAt,
        CancellationToken cancellationToken);

    Task<DailyCalendar?> Getday(Guid Id, CancellationToken cancellationToken);
}