using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

public sealed record FailedLoginRegisteredEvent(
    UserId UserId,
    int FailureCount
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}