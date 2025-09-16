namespace SmartAttendance.Application.Features.Plans.Commands;

public sealed class EnrollStudentCommandHandler(
    IPlanRepository planRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<EnrollStudentCommand>
{
    public async Task Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
    {
        var plan = await planRepository.GetByIdAsync(new PlanId(request.PlanId), cancellationToken) ?? throw new KeyNotFoundException("طرح مورد نظر یافت نشد.");
        plan.RegisterStudent(new UserId(request.StudentId));
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}