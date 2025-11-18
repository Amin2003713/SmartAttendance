using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Notifications.Commands;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums.Plans.Enrollment;
using SmartAttendance.Domain.Features.PlanEnrollments;

namespace SmartAttendance.RequestHandlers.Features.Plans.Commands.Create;

public class CreatePlanEnrollmentCommandHandler(
    IPlanEnrollmentCommandRepository enrollmentCommandRepo,
    IPlanQueryRepository planQueryRepo,
    IPlanEnrollmentQueryRepository enrollmentQueryRepo,
    IMediator mediator
) : IRequestHandler<CreatePlanEnrollmentCommand, Guid>
{
    public async Task<Guid> Handle(CreatePlanEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var plan = await planQueryRepo.GetByIdAsync(cancellationToken, request.PlanId);
        if (plan == null)
            throw SmartAttendanceException.NotFound("Plan not found");

        var activeEnrollmentsCount = await enrollmentQueryRepo.TableNoTracking
            .CountAsync(e => e.PlanId == request.PlanId && e.Status == EnrollmentStatus.Active, cancellationToken);

        var status = activeEnrollmentsCount >= plan.Capacity ? EnrollmentStatus.Waitlisted : EnrollmentStatus.Active;

        var enrollment = new PlanEnrollment
        {
            PlanId = request.PlanId,
            StudentId = request.StudentId,
            Status = status,
            EnrolledAt = DateTime.UtcNow
        };

        await enrollmentCommandRepo.AddAsync(enrollment, cancellationToken);

        
            await mediator.Send(new NotifyEnrollmentStatusChangedCommand
                {
                    StudentId = request.StudentId,
                    PlanId = plan.CourseName,
                    Status = status
                },
                cancellationToken);
        

        return enrollment.Id;
    }
}