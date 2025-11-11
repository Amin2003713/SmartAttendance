using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Notifications.Commands;
using SmartAttendance.Application.Features.Plans.Commands.ReschedulePlan;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Plans.Commands.Create;

public class ReschedulePlanCommandHandler(
    IPlanEnrollmentQueryRepository enrollmentQueryRepository,
    IPlanCommandRepository commandRepository,
    IMediator mediator ,
    IPlanQueryRepository queryRepository
) : IRequestHandler<ReschedulePlanCommand>
{
    public async Task Handle(ReschedulePlanCommand request, CancellationToken cancellationToken)
    {
        // Fetch the existing plan
        var existingPlan = await queryRepository.GetByIdAsync( cancellationToken , request.Id);
        if (existingPlan == null)
            throw SmartAttendanceException.NotFound("Plan not found");

        // Update schedule
        existingPlan.StartTime = request.Start;
        existingPlan.EndTime = request.End;

        var enr = await enrollmentQueryRepository.TableNoTracking.Where(a => a.PlanId == existingPlan.Id).ToListAsync(cancellationToken);
        var to  = enr.Select(a => a.StudentId).ToList() ?? [];
        to.AddRange(existingPlan.Teacher.Select(a => a.TeacherId).ToList());
        if (enr.Count != 0)
            await mediator.Send(new NotifyTimeHasChengedCommand()
                {
                    PlanId = existingPlan.Id,
                    NewDateEnd = request.End,
                    NewDateStart = request.Start,
                    ToUser = to
                } ,
                cancellationToken);

        // Save changes
        await commandRepository.UpdateAsync(existingPlan, cancellationToken);
    }
}