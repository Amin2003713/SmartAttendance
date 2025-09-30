using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Common.General.Enums.Plans.Enrollment;
using SmartAttendance.Domain.Features.Attendances;

namespace SmartAttendance.Domain.Features.Plans;

public class Plan : BaseEntity
{
    public string CourseName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid TeacherId { get; set; }
    public string Location { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }

    // Navigation
    public User Teacher { get; set; } = default!;
    public ICollection<PlanEnrollment> Enrollments { get; set; } = new List<PlanEnrollment>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}

public class PlanEnrollment : BaseEntity
{
    public Guid PlanId { get; set; }
    public Guid StudentId { get; set; }
    public EnrollmentStatus Status { get; set; } 
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Plan Plan { get; set; } = default!;
    public User Student { get; set; } = default!;
}