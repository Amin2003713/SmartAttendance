using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Calenders.CalenderUsers;

namespace SmartAttendance.Application.Interfaces.Calendars.CalendarUsers;

public interface ICalendarUserCommandRepository : ICommandRepository<CalendarUser>,
    IScopedDependency { }