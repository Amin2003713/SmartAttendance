using Mapster;
using SmartAttendance.Application.Features.Majors.Queries.GetById;
using SmartAttendance.Application.Features.Plans.Commands.Create;
using SmartAttendance.Application.Features.Subjects.Queries;
using SmartAttendance.Application.Features.Subjects.Queries.ByIds;
using SmartAttendance.Application.Features.Users.Queries.GetByIds;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Features.Plans;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.RequestHandlers.Features.Plans.Commands.Create;

public class CreatePlanCommandHandler(
    IPlanCommandRepository commandRepository ,
    Mediator mediator
) : IRequestHandler<CreatePlanCommand>
{
    public async Task Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        var major   =  await mediator.Send(new GetMajorById(request.MajorId), cancellationToken);
        if (major == null)
            throw SmartAttendanceException.NotFound("Major not found");


        var subjects = await mediator.Send(new GetSubjectByIdsQuery(request.SubjectIds), cancellationToken);
        if (subjects.Count != request.SubjectIds.Count)
            throw SmartAttendanceException.NotFound("one of the subjects not found");

        var teachers =  await mediator.Send(new GetTeacherByIds(request.TeacherIds), cancellationToken);
        if (teachers.Count != request.TeacherIds.Count)
            throw SmartAttendanceException.NotFound("one of the teachers not found");

        var plan = request.Adapt<Plan>();
        plan.Teacher = teachers.Select(a => new TeacherPlan
            {
                PlanId = plan.Id,
                TeacherId = a.Id
            })
            .ToList();

        plan.Subjects = subjects.Select(a => new SubjectPlans
            {
                PlanId = plan.Id,
                SubjectId =  a.Id
            })
            .ToList();

        await commandRepository.AddAsync(plan , cancellationToken) ;
    }
}