using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

// رویدادهای دامنه مرتبط با کاربر
public sealed record RoleAssignedEvent(
    UserId UserId,
    RoleId RoleId
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}