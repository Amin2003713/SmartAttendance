using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Notifications;

namespace SmartAttendance.Application.Interfaces.Notifications;

public interface INotificationQueryRepository : IQueryRepository<Notification>,
    IScopedDependency { }