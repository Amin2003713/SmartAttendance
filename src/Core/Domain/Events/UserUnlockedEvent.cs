using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

public sealed record UserUnlockedEvent(
    UserId UserId
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}