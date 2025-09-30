using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Common.General.Enums.Plans.Enrollment;

namespace SmartAttendance.Domain.Features.Plans;

public class PlanEnrollment : BaseEntity
{
    public Guid PlanId { get; set; }
    public Guid StudentId { get; set; }
    public EnrollmentStatus Status { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Plan Plan { get; set; } = null!;
    public User Student { get; set; } = null!;
}