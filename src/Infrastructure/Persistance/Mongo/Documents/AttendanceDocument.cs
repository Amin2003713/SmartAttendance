namespace SmartAttendance.Persistence.Mongo.Documents;

public sealed class AttendanceDocument
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid PlanId { get; set; }
    public string Status { get; set; } = "Unknown";
    public DateTime? RecordedAtUtc { get; set; }
    public bool IsExcused { get; set; }
    public string? ExcusalReason { get; set; }
    public double? Points { get; set; }
}