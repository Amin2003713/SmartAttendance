using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Domain.Features.Majors;

public class TeacherPlan  : BaseEntity
{
    public Guid PlanId { get; set; }
    public Plan Plan { get; set; } = default!;


    public Guid TeacherId { get; set; }
    public User Teacher { get; set; } = default!;
}