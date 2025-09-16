using SmartAttendance.Application.Features.Plans.Responses;

namespace SmartAttendance.Application.Features.Plans.Queries;

public sealed class GetPlanByIdQueryHandler(
    IPlanRepository planRepository
) : IRequestHandler<GetPlanByIdQuery, PlanDto>
{
    public async Task<PlanDto> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await planRepository.GetByIdAsync(new PlanId(request.PlanId), cancellationToken) ?? throw new KeyNotFoundException("طرح مورد نظر یافت نشد.");
        return plan.Adapt<PlanDto>();
    }
}