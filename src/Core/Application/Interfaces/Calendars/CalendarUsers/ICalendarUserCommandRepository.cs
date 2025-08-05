using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Calenders.CalenderUsers;

namespace Shifty.Application.Interfaces.Calendars.CalendarUsers;

public interface ICalendarUserCommandRepository : ICommandRepository<CalendarUser>,
    IScopedDependency { }