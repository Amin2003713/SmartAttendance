using Shifty.Application.Commons.Base;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Calenders.CalenderProjects;

namespace Shifty.Application.Interfaces.Calendars.CalendarProjects;

public interface ICalendarProjectCommandRepository : ICommandRepository<CalendarProject>,
    IScopedDependency { }