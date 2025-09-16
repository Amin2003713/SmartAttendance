using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

public sealed record DocumentRejectedEvent(
    DocumentId DocumentId
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}