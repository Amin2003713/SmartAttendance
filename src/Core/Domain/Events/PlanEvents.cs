using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

// رویدادهای دامنه مرتبط با طرح
public sealed record StudentRegisteredToPlanEvent(PlanId PlanId, UserId StudentId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record EnrollmentCanceledEvent(PlanId PlanId, UserId StudentId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record PlanCapacityReachedEvent(PlanId PlanId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

