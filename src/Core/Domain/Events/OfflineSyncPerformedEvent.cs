using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

public sealed record OfflineSyncPerformedEvent(
    AttendanceId AttendanceId
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}