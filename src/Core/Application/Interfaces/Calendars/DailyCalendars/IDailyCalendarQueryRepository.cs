using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Calendars.Request.Queries.GetHoliday;
using Shifty.Application.Calendars.Request.Queries.GetReminder;
using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Calenders.DailyCalender;

namespace Shifty.Application.Interfaces.Calendars.DailyCalendars;

public interface IDailyCalendarQueryRepository : IQueryRepository<DailyCalendar>,
    IScopedDependency
{
    Task<List<DailyCalendar>?> GetCustomCalendarEvents(
        DateTime startDate,
        DateTime date,
       
        Guid userId,
        CancellationToken cancellationToken);

    Task<bool> IsAlreadyHoliday( DateTime dateTime, CancellationToken cancellationToken);

    Task<List<GetHolidayResponse>> GetHolidaysForMonth(
    
        DateTime startAt,
        DateTime endAt,
        CancellationToken cancellationToken);

    Task<List<GetReminderResponse>> GetReminderForProject(
       
        Guid userId,
        DateTime startAt,
        DateTime endAt,
        CancellationToken cancellationToken);

    Task<DailyCalendar?> Getday(Guid Id, CancellationToken cancellationToken);
}