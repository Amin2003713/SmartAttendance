using Shifty.Application.Commons.Base;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Calenders.CalenderUsers;

namespace Shifty.Application.Interfaces.Calendars.CalendarUsers;

public interface ICalendarUserCommandRepository : ICommandRepository<CalendarUser>,
    IScopedDependency { }