namespace SmartAttendance.Application.Features.Attendance.Responses;

// DTO نمایش حضور
public sealed class AttendanceDto
{
	public Guid Id { get; init; }
	public Guid StudentId { get; init; }
	public Guid PlanId { get; init; }
	public string Status { get; init; } = string.Empty;
	public DateTime? RecordedAtUtc { get; init; }
	public double? Points { get; init; }
}

