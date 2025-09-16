using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

// رویدادهای دامنه مرتبط با طرح
public sealed record StudentRegisteredToPlanEvent(
    PlanId PlanId,
    UserId StudentId
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}