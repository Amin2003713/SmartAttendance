using Shifty.Application.Commons.Base;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Calenders.DailyCalender;

namespace Shifty.Application.Interfaces.Calendars.DailyCalendars;

public interface IDailyCalendarCommandRepository : ICommandRepository<DailyCalendar>,
    IScopedDependency { }