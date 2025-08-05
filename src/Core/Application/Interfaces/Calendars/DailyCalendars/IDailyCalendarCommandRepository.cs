using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Calenders.DailyCalender;

namespace Shifty.Application.Interfaces.Calendars.DailyCalendars;

public interface IDailyCalendarCommandRepository : ICommandRepository<DailyCalendar>,
    IScopedDependency { }