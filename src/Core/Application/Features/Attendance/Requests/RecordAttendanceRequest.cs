namespace SmartAttendance.Application.Features.Attendance.Requests;

// درخواست ثبت حضور
public sealed class RecordAttendanceRequest
{
    public Guid AttendanceId { get; init; }
    public Guid StudentId { get; init; }
    public Guid PlanId { get; init; }
    public string? QrToken { get; init; }
    public DateTime? QrExpiresAtUtc { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public double? AllowedRadiusMeters { get; init; }
    public Guid? ApproverId { get; init; }
    public bool? ManualPresent { get; init; }
}