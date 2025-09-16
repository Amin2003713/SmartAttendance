using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

public sealed record RoleRemovedEvent(
    UserId UserId,
    RoleId RoleId
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}