using SmartAttendance.Domain.PlanAggregate;

namespace SmartAttendance.Application.Features.Plans.Commands;

public sealed class UpdatePlanCommandHandler(
    IPlanRepository repo,
    IUnitOfWork uow
) : IRequestHandler<UpdatePlanCommand>
{
    public async Task Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await repo.GetByIdAsync(new PlanId(request.PlanId), cancellationToken) ?? throw new KeyNotFoundException("طرح یافت نشد.");
        plan.Reschedule(request.Request.StartsAtUtc, request.Request.EndsAtUtc);
        // ظرفیت جدید
        plan = new PlanAggregate(plan.Id,
            request.Request.Title,
            request.Request.Description,
            plan.StartsAtUtc,
            plan.EndsAtUtc,
            new PlanCapacity(request.Request.Capacity, plan.Capacity.Reserved));

        await repo.UpdateAsync(plan, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
    }
}