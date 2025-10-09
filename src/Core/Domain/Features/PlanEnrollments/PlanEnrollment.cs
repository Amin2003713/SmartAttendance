using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Common.General.Enums.Plans.Enrollment;
using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Domain.Features.PlanEnrollments;

public class PlanEnrollment : BaseEntity
{
    public Guid PlanId { get; set; }
    public Guid StudentId { get; set; }
    public Guid? AttendanceId { get; set; }
    public EnrollmentStatus Status { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    public Plan Plan { get; set; } = null!;
    public User Student { get; set; } = null!;
    public Attendance? Attendance { get; set; } = null!;
}