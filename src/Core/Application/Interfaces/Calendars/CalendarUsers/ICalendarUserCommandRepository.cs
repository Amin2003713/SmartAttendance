using SmartAttendance.Domain.Calenders.CalenderUsers;

namespace SmartAttendance.Application.Interfaces.Calendars.CalendarUsers;

public interface ICalendarUserCommandRepository : ICommandRepository<CalendarUser>,
                                                  IScopedDependency { }