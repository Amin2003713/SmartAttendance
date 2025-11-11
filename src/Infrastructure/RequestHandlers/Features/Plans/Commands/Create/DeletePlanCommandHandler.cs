using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Notifications.Commands;
using SmartAttendance.Application.Features.Plans.Commands.Delete;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Plans.Commands.Create;

public class DeletePlanCommandHandler(
    IPlanCommandRepository commandRepository,
    IPlanEnrollmentQueryRepository enrollmentQueryRepository,
    IPlanQueryRepository queryRepository,
    IMediator mediator
) : IRequestHandler<DeletePlanCommand>
{
    public async Task Handle(DeletePlanCommand request, CancellationToken cancellationToken)
    {
        var existingPlan = await queryRepository.GetByIdAsync(cancellationToken, request.Id);
        if (existingPlan == null)
            throw SmartAttendanceException.NotFound("Plan not found");

        // Collect users to notify before deletion
        var enrollments = await enrollmentQueryRepository.TableNoTracking
            .Where(e => e.PlanId == existingPlan.Id)
            .ToListAsync(cancellationToken);

        var affectedUserIds = enrollments.Select(e => e.StudentId).ToList();
        affectedUserIds.AddRange(existingPlan.Teacher.Select(t => t.TeacherId));

        // Delete plan
        await commandRepository.DeleteAsync(existingPlan, cancellationToken);

        // Notify all users
        if (affectedUserIds.Count > 0)
        {
            await mediator.Send(new NotifyPlanDeletedCommand
                {
                    PlanId = request.Id,
                    ToUser = affectedUserIds
                },
                cancellationToken);
        }
    }
}