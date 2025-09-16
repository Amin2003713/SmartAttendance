namespace SmartAttendance.Application.Features.Plans.Commands;

// Command: ثبت‌نام دانشجو در طرح
public sealed record EnrollStudentCommand(Guid PlanId, Guid StudentId) : IRequest;

// Handler: ثبت‌نام دانشجو
public sealed class EnrollStudentCommandHandler(IPlanRepository planRepository, IUnitOfWork unitOfWork) : IRequestHandler<EnrollStudentCommand>
{
	public async Task Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
	{
		var plan = await planRepository.GetByIdAsync(new PlanId(request.PlanId), cancellationToken)
			?? throw new KeyNotFoundException("طرح مورد نظر یافت نشد.");
		plan.RegisterStudent(new UserId(request.StudentId));
		await unitOfWork.SaveChangesAsync(cancellationToken);
	}
}

