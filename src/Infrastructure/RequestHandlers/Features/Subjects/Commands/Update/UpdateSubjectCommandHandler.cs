using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Subjects.Commands.Update;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Domain.Features.Subjects;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Subjects.Commands.Update;

public class UpdateSubjectCommandHandler(
    IdentityService  identityService,
    ISubjectQueryRepository queryRepository ,
    ISubjectCommandRepository commandRepository ,
    ISubjectTeacherCommandRepository STCommandRepo ,
    ISubjectTeacherQueryRepository SubjectQueryRepository
) : IRequestHandler<UpdateSubjectCommand>
{
    public async Task Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        if (identityService.GetRoles() is  Roles.Student)
            throw SmartAttendanceException.Forbidden("cannot access");

        var subject = await queryRepository.FirstOrDefaultsAsync(a => a.Name.Trim().Equals(request.Name.Trim()), cancellationToken);

        if (subject is null)
            throw SmartAttendanceException.NotFound();

        var currentTeachers = await SubjectQueryRepository.TableNoTracking
            .Where(tp => tp.Id == request.Id)
            .ToListAsync(cancellationToken);

        var teachersToAdd = request.TeacherIds
            .Where(tid => currentTeachers.All(ct => ct.TeacherId != tid))
            .Select(tid => new SubjectTeacher()
            {
                SubjectId = request.Id,
                TeacherId = tid
            })
            .ToList();

        var teachersToRemove = currentTeachers
            .Where(ct => !request.TeacherIds.Contains(ct.TeacherId))
            .ToList();

        if (teachersToRemove.Count != 0)
        {
            await STCommandRepo.DeleteRangeAsync(teachersToRemove, cancellationToken);
        }

        if (teachersToAdd.Count != 0)
        {
            await STCommandRepo.AddRangeAsync(teachersToAdd, cancellationToken);
        }

        subject.Name = request.Name.Trim();


        await commandRepository.UpdateAsync(subject, cancellationToken);
    }
}