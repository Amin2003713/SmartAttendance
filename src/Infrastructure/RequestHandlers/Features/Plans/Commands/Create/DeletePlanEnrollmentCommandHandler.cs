using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Notifications.Commands;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums.Plans.Enrollment;

namespace SmartAttendance.RequestHandlers.Features.Plans.Commands.Create;

public class DeletePlanEnrollmentCommandHandler(
    IPlanEnrollmentCommandRepository enrollmentCommandRepo,
    IPlanEnrollmentQueryRepository enrollmentQueryRepo,
    IMediator mediator
) : IRequestHandler<DeletePlanEnrollmentCommand>
{
    public async Task Handle(DeletePlanEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await enrollmentQueryRepo.Table.Include(a=>a.Plan)
            .FirstOrDefaultAsync(e => e.PlanId == request.PlanId && e.StudentId == request.StudentId, cancellationToken);

        if (enrollment == null)
            throw SmartAttendanceException.NotFound("Enrollment not found");

        // Delete enrollment
        await enrollmentCommandRepo.DeleteAsync(enrollment, cancellationToken);
        // Notify student about deletion
        await mediator.Send(new NotifyEnrollmentStatusChangedCommand
            {
                StudentId = request.StudentId,
                PlanId = enrollment.Plan.CourseName,
                Status = EnrollmentStatus.Cancelled
            },
            cancellationToken);

        // Promote oldest waitlisted enrollment if exists
        var oldestWaitlisted = await enrollmentQueryRepo.Table.Include(a=>a.Plan)
            .Where(e => e.PlanId == request.PlanId && e.Status == EnrollmentStatus.Waitlisted)
            .OrderBy(e => e.EnrolledAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (oldestWaitlisted != null)
        {
            oldestWaitlisted.Status = EnrollmentStatus.Active;
            await enrollmentCommandRepo.UpdateAsync(oldestWaitlisted, cancellationToken);

            await mediator.Send(new NotifyEnrollmentStatusChangedCommand
                {
                    StudentId = oldestWaitlisted.StudentId,
                    PlanId = oldestWaitlisted.Plan.CourseName,
                    Status = EnrollmentStatus.Active
                },
                cancellationToken);
        }
    }
}