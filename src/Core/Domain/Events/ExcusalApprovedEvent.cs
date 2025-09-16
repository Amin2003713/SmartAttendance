using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

public sealed record ExcusalApprovedEvent(
    AttendanceId AttendanceId,
    string Reason
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}