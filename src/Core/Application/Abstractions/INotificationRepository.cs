using SmartAttendance.Domain.NotificationAggregate;

namespace SmartAttendance.Application.Abstractions;

public interface INotificationRepository
{
    Task<Notification?>               GetByIdAsync(NotificationId id, CancellationToken ct = default);
    Task                              AddAsync(Notification notification, CancellationToken ct = default);
    Task                              UpdateAsync(Notification notification, CancellationToken ct = default);
    Task                              DeleteAsync(NotificationId id, CancellationToken ct = default);
    Task<IReadOnlyList<Notification>> GetByUserAsync(UserId userId, CancellationToken ct = default);
}