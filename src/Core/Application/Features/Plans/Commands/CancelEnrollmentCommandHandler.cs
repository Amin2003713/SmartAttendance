namespace SmartAttendance.Application.Features.Plans.Commands;

public sealed class CancelEnrollmentCommandHandler(
    IPlanRepository repo,
    IUnitOfWork uow
) : IRequestHandler<CancelEnrollmentCommand>
{
    public async Task Handle(CancelEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var plan = await repo.GetByIdAsync(new PlanId(request.PlanId), cancellationToken) ?? throw new KeyNotFoundException("طرح یافت نشد.");
        plan.CancelEnrollment(new UserId(request.StudentId));
        await repo.UpdateAsync(plan, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
    }
}