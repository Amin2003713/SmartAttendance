using SmartAttendance.Application.Features.Plans.Responses;

namespace SmartAttendance.Application.Features.Plans.Queries;

// Query: دریافت اطلاعات طرح
public sealed record GetPlanByIdQuery(
    Guid PlanId
) : IRequest<PlanDto>;

// Handler: دریافت اطلاعات طرح