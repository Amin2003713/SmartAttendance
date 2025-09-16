using SmartAttendance.Domain.Common;
using SmartAttendance.Domain.ValueObjects;

namespace SmartAttendance.Domain.Events;

// رویدادهای دامنه مرتبط با حضور و غیاب
public sealed record AttendanceRecordedEvent(AttendanceId AttendanceId, UserId StudentId, PlanId PlanId, AttendanceStatus Status) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record ExcusalApprovedEvent(AttendanceId AttendanceId, string Reason) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record PointsCalculatedEvent(AttendanceId AttendanceId, double Points) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record OfflineSyncPerformedEvent(AttendanceId AttendanceId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record QRValidatedEvent(AttendanceId AttendanceId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record GPSValidatedEvent(AttendanceId AttendanceId) : IDomainEvent
{
	public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

