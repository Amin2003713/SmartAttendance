using SmartAttendance.Domain.Common;
using SmartAttendance.Domain.ValueObjects;

namespace SmartAttendance.Domain.Events;

// رویدادهای دامنه مرتبط با حضور و غیاب
public sealed record AttendanceRecordedEvent(
    AttendanceId AttendanceId,
    UserId StudentId,
    PlanId PlanId,
    AttendanceStatus Status
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}