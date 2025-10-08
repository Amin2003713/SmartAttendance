using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.Majors;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Domain.Features.Plans;

public class Plan : BaseEntity
{
    public string CourseName { get; set; } = default!;

    public string Description { get; set; } = default!;
    public string Location { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }

    public string Address { get; set; } = default!;

    // Navigation
    public virtual Guid? MajorId { get; set; }
    public virtual Major? Major { get; set; }

    public ICollection<SubjectPlans> Subjects { get; set; } = [];
    public ICollection<TeacherPlan> Teacher { get; set; } = [];

    public ICollection<PlanEnrollment> Enrollments { get; set; } = new List<PlanEnrollment>();
}