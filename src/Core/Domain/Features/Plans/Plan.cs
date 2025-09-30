using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Domain.Features.Plans;

public class Plan : BaseEntity
{
    public string CourseName { get; set; } = default!;

    public Guid MajorId { get; set; }
    public string Description { get; set; } = default!;
    public string Location { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }

    public string Address { get; set; } = default!;

    // Navigation
    public ICollection<MajorPlans> Majors { get; set; } = [];
    public ICollection<TeacherPlan> Teacher { get; set; } = [];

    public ICollection<PlanEnrollment> Enrollments { get; set; } = new List<PlanEnrollment>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}