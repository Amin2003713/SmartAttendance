using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Calenders.DailyCalender;

namespace SmartAttendance.Application.Interfaces.Calendars.DailyCalendars;

public interface IDailyCalendarCommandRepository : ICommandRepository<DailyCalendar>,
                                                   IScopedDependency;