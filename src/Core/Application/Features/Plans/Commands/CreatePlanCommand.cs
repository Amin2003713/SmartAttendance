using SmartAttendance.Application.Features.Plans.Requests;
using SmartAttendance.Application.Features.Plans.Responses;
using SmartAttendance.Domain.PlanAggregate;

namespace SmartAttendance.Application.Features.Plans.Commands;

// Command: ایجاد طرح
public sealed record CreatePlanCommand(CreatePlanRequest Request) : IRequest<PlanDto>;

// Handler: ایجاد طرح
public sealed class CreatePlanCommandHandler(
	IPlanRepository planRepository,
	IUnitOfWork unitOfWork
) : IRequestHandler<CreatePlanCommand, PlanDto>
{
	public async Task<PlanDto> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
	{
		var r = request.Request;
		var capacity = new PlanCapacity(r.Capacity);
		if (r.EndsAtUtc <= r.StartsAtUtc) throw new DomainValidationException("تاریخ پایان باید بعد از شروع باشد.");

		var plan = new PlanAggregate(PlanId.New(), r.Title, r.Description, r.StartsAtUtc, r.EndsAtUtc, capacity);
		await planRepository.AddAsync(plan, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		return plan.Adapt<PlanDto>();
	}
}

