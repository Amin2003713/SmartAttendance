using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

// رویدادهای دامنه مرتبط با کاربر
public sealed record RoleAssignedEvent(UserId UserId, RoleId RoleId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record RoleRemovedEvent(UserId UserId, RoleId RoleId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record FailedLoginRegisteredEvent(UserId UserId, int FailureCount) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record UserLockedEvent(UserId UserId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record UserUnlockedEvent(UserId UserId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

