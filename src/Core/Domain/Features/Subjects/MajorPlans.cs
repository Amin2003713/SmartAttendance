using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Domain.Features.Subjects;

public class SubjectPlans  : BaseEntity
{
    public Guid PlanId { get; set; }
    public Plan Plan { get; set; } = default!;


    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; } = default!;
}