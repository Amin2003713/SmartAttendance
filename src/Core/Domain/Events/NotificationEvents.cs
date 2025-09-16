using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

// رویدادهای مربوط به اعلان‌ها
public sealed record NotificationMarkedAsReadEvent(NotificationId NotificationId, UserId RecipientId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

