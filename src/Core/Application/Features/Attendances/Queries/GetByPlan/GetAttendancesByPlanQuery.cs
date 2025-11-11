using SmartAttendance.Application.Features.Attendances.Responses;

namespace SmartAttendance.Application.Features.Attendances.Queries.GetByPlan;

public class GetAttendancesByPlanQuery : IRequest<List<GetAttendanceInfoResponse>>
{
    public Guid PlanId { get; set; }
}