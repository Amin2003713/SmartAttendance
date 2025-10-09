using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Majors.Commands.DeActive;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Majors.Commands.DeActive;

public class DeActiveMajorCommandHandler(
    IMajorQueryRepository queryRepository ,
    IMajorCommandRepository commandRepository
)  : IRequestHandler<DeActiveMajorCommand>
{
    public async Task Handle(DeActiveMajorCommand request, CancellationToken cancellationToken)
    {
        var major = await queryRepository.TableNoTracking.FirstOrDefaultAsync(a => a.Id == request.Id ,
            cancellationToken: cancellationToken);

        if (major is null )
            throw SmartAttendanceException.NotFound();

        await commandRepository.DeleteAsync(major , cancellationToken);
    }
}