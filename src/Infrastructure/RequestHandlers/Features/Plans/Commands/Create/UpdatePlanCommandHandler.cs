using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Majors.Queries.GetById;
using SmartAttendance.Application.Features.Notifications.Commands;
using SmartAttendance.Application.Features.Plans.Commands.Update;
using SmartAttendance.Application.Features.Subjects.Queries.ByIds;
using SmartAttendance.Application.Features.Users.Queries.GetByIds;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.RequestHandlers.Features.Plans.Commands.Create;

public class UpdatePlanCommandHandler(
    IPlanCommandRepository commandRepository,
    IPlanEnrollmentQueryRepository enrollmentQueryRepository,
    IPlanQueryRepository queryRepository,
    IMediator mediator
) : IRequestHandler<UpdatePlanCommand>
{
    public async Task Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        var enrollmentCount = await enrollmentQueryRepository.TableNoTracking
            .CountAsync(a => a.PlanId == request.Id, cancellationToken);

        if (enrollmentCount is not 0 && enrollmentCount > request.Capacity)
            throw SmartAttendanceException.BadRequest("Capacity is exceeded");

        // Fetch existing plan
        var existingPlan = await queryRepository.GetByIdAsync(cancellationToken, request.Id);
        if (existingPlan == null)
            throw SmartAttendanceException.NotFound("Plan not found");

        // Validate major
        var major = await mediator.Send(new GetMajorById(request.MajorId), cancellationToken);
        if (major == null)
            throw SmartAttendanceException.NotFound("Major not found");

        // Validate subjects
        var subjects = await mediator.Send(new GetSubjectByIdsQuery(request.SubjectIds), cancellationToken);
        if (subjects.Count != request.SubjectIds.Count)
            throw SmartAttendanceException.NotFound("One of the subjects not found");

        // Validate teachers
        var teachers = await mediator.Send(new GetTeacherByIds(request.TeacherIds), cancellationToken);
        if (teachers.Count != request.TeacherIds.Count)
            throw SmartAttendanceException.NotFound("One of the teachers not found");

        // Detect changes in schedule/location
        var isTimeChanged =
            existingPlan.StartTime != request.StartTime ||
            existingPlan.EndTime != request.EndTime;

        var isLocationChanged =
            existingPlan.Address != request.Address ||
            !existingPlan.Location.Equals(request.Location);

        // Map updates
        request.Adapt(existingPlan);

        // Rebuild teacher and subject relationships
        existingPlan.Teacher = teachers.Select(t => new TeacherPlan
            {
                PlanId = existingPlan.Id,
                TeacherId = t.Id
            })
            .ToList();

        existingPlan.Subjects = subjects.Select(s => new SubjectPlans
            {
                PlanId = existingPlan.Id,
                SubjectId = s.Id
            })
            .ToList();

        await commandRepository.UpdateAsync(existingPlan, cancellationToken);

        // If key details changed, notify affected users
        if (isTimeChanged || isLocationChanged)
        {
            var enrollments = await enrollmentQueryRepository.TableNoTracking
                .Where(e => e.PlanId == existingPlan.Id)
                .ToListAsync(cancellationToken);

            var affectedUserIds = enrollments.Select(e => e.StudentId).ToList();
            affectedUserIds.AddRange(existingPlan.Teacher.Select(t => t.TeacherId));

            await mediator.Send(new NotifyPlanUpdatedCommand
                {
                    PlanId = existingPlan.Id,
                    ToUser = affectedUserIds,
                    IsTimeChanged = isTimeChanged,
                    IsLocationChanged = isLocationChanged,
                    NewStart = existingPlan.StartTime,
                    NewEnd = existingPlan.EndTime,
                    NewLocation = existingPlan.Location,
                    NewAddress = existingPlan.Address
                },
                cancellationToken);
        }
    }
}