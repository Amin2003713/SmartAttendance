using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

public sealed record GPSValidatedEvent(
    AttendanceId AttendanceId
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}