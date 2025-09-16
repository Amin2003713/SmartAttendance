using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

public sealed record PlanCapacityReachedEvent(
    PlanId PlanId
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}