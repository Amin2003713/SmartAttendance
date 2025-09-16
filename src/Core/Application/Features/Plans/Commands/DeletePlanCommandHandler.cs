namespace SmartAttendance.Application.Features.Plans.Commands;

public sealed class DeletePlanCommandHandler(
    IPlanRepository repo,
    IUnitOfWork uow
) : IRequestHandler<DeletePlanCommand>
{
    public async Task Handle(DeletePlanCommand request, CancellationToken cancellationToken)
    {
        await repo.DeleteAsync(new PlanId(request.PlanId), cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
    }
}