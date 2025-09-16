using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Events;

public sealed record PointsCalculatedEvent(
    AttendanceId AttendanceId,
    double Points
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}