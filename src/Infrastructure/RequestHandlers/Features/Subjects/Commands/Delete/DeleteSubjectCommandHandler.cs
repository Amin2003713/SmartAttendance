using SmartAttendance.Application.Features.Subjects.Commands.Delete;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Subjects.Commands.Delete;

public record DeleteSubjectCommandHandler(
    ISubjectCommandRepository CommandRepository ,
    ISubjectQueryRepository SubjectQueryRepository
) : IRequestHandler<DeleteSubjectCommand>
{
    public async Task Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        var sub = await SubjectQueryRepository.FirstOrDefaultsAsync(a => a.Id == request.Id , cancellationToken);
        if (sub == null)
            throw SmartAttendanceException.NotFound();

        await CommandRepository.DeleteAsync(sub  , cancellationToken);
    }
}