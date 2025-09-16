using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

public sealed record EnrollmentCanceledEvent(
    PlanId PlanId,
    UserId StudentId
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}