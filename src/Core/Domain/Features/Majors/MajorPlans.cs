using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Domain.Features.Majors;

public class MajorPlans  : BaseEntity
{
    public Guid PlanId { get; set; }
    public Plan Plan { get; set; } = default!;


    public Guid MajorId { get; set; }
    public Major Major { get; set; } = default!;
}