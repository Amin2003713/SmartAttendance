using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Calenders.CalenderProjects;

namespace Shifty.Application.Interfaces.Calendars.CalendarProjects;

public interface ICalendarProjectCommandRepository : ICommandRepository<CalendarProject>,
    IScopedDependency { }