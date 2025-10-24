using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Subjects.Commands;
using SmartAttendance.Application.Features.Subjects.Commands.Create;
using SmartAttendance.Application.Features.Subjects.Commands.Update;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Application.Interfaces.Users;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Domain.Features.Subjects;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Subjects.Commands.Create;

public class CreateSubjectCommandHandler(
    IdentityService  identityService,
    ISubjectQueryRepository queryRepository ,
    ISubjectCommandRepository commandRepository ,
    IUserQueryRepository userQueryRepository,
    IMediator mediator,
    IMajorQueryRepository MajorRepository
) : IRequestHandler<CreateSubjectCommand>
{
    public async Task Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
    {
        (Guid id , Roles? role ) user = (identityService.GetUserId<Guid>() , identityService.GetRoles());


        if (!await MajorRepository.AnyAsync(a => a.Id == request.MajorId, cancellationToken))
            throw SmartAttendanceException.NotFound();

        var subject = await queryRepository.FirstOrDefaultsAsync(a => a.Name.Trim().Equals(request.Name.Trim()), cancellationToken);

        if (subject is not null)
        {
            await mediator.Send(new  UpdateSubjectCommand()
                {
                    TeacherIds = request.TeacherIds,
                    Id = subject.Id,
                    Name = request.Name,
                    MajorId = request.MajorId
                },
                cancellationToken);

            return;
        }

        if (await userQueryRepository.TableNoTracking.AllAsync(a => request.TeacherIds.Contains(a.Id), cancellationToken))
            throw SmartAttendanceException.NotFound();


        var newSub = new Subject ()
        {
            MajorId = request.MajorId,
            Name = request.Name.Trim(),
            Teachers = request.TeacherIds.Select(a => new SubjectTeacher()
                {
                    TeacherId = a
                })
                .ToList()
        }   ;

        await commandRepository.AddAsync(newSub , cancellationToken);
    }
}