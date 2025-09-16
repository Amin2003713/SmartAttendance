using SmartAttendance.Application.Features.Plans.Requests;
using SmartAttendance.Domain.PlanAggregate;

namespace SmartAttendance.Application.Features.Plans.Commands;

public sealed record UpdatePlanCommand(Guid PlanId, UpdatePlanRequest Request) : IRequest;
public sealed record DeletePlanCommand(Guid PlanId) : IRequest;
public sealed record CancelEnrollmentCommand(Guid PlanId, Guid StudentId) : IRequest;

public sealed class UpdatePlanCommandHandler(IPlanRepository repo, IUnitOfWork uow) : IRequestHandler<UpdatePlanCommand>
{
	public async Task Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
	{
		var plan = await repo.GetByIdAsync(new PlanId(request.PlanId), cancellationToken) ?? throw new KeyNotFoundException("طرح یافت نشد.");
		plan.Reschedule(request.Request.StartsAtUtc, request.Request.EndsAtUtc);
		// ظرفیت جدید
		plan = new PlanAggregate(plan.Id, request.Request.Title, request.Request.Description, plan.StartsAtUtc, plan.EndsAtUtc, new PlanCapacity(request.Request.Capacity, plan.Capacity.Reserved));
		await repo.UpdateAsync(plan, cancellationToken);
		await uow.SaveChangesAsync(cancellationToken);
	}
}

public sealed class DeletePlanCommandHandler(IPlanRepository repo, IUnitOfWork uow) : IRequestHandler<DeletePlanCommand>
{
	public async Task Handle(DeletePlanCommand request, CancellationToken cancellationToken)
	{
		await repo.DeleteAsync(new PlanId(request.PlanId), cancellationToken);
		await uow.SaveChangesAsync(cancellationToken);
	}
}

public sealed class CancelEnrollmentCommandHandler(IPlanRepository repo, IUnitOfWork uow) : IRequestHandler<CancelEnrollmentCommand>
{
	public async Task Handle(CancelEnrollmentCommand request, CancellationToken cancellationToken)
	{
		var plan = await repo.GetByIdAsync(new PlanId(request.PlanId), cancellationToken) ?? throw new KeyNotFoundException("طرح یافت نشد.");
		plan.CancelEnrollment(new UserId(request.StudentId));
		await repo.UpdateAsync(plan, cancellationToken);
		await uow.SaveChangesAsync(cancellationToken);
	}
}

