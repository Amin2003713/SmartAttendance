using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Majors.Commands.Create;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.RequestHandlers.Features.Majors.Commands.Create;

public class CreateMajorCommandHandler(
    IMajorQueryRepository queryRepository ,
    IMajorCommandRepository commandRepository
)  : IRequestHandler<CreateMajorCommand>
{
    public async Task Handle(CreateMajorCommand request, CancellationToken cancellationToken)
    {
        if (await queryRepository.TableNoTracking.AnyAsync(a => a.Name == request.Name && a.HeadMasterId == request.HeadMasterId,
                cancellationToken: cancellationToken))
            throw SmartAttendanceException.Conflict();

        await commandRepository.AddAsync(request.Adapt<Major>() , cancellationToken);
    }
}