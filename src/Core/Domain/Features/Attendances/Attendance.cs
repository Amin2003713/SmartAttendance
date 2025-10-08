using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Common.General.Enums.Attendance;
using SmartAttendance.Domain.Features.Excuses;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Domain.Features.Attendances;

public class Attendance  : BaseEntity
{
    public Guid EnrollmentId { get; set; }
    public Guid StudentId { get; set; }
    public AttendanceStatus Status { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    public Guid? ExcuseId { get; set; }

    // Navigation
    public PlanEnrollment Enrollment { get; set; } = default!;
    public User Student { get; set; } = default!;
    public Excuse? Excuse { get; set; }
}