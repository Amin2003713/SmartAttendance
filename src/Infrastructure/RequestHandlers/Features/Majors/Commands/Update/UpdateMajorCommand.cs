using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.Application.Features.Majors.Commands.Update;

public class UpdateMajorCommandHandler(
    IMajorQueryRepository queryRepository,
    IMajorCommandRepository commandRepository
) : IRequestHandler<UpdateMajorCommand>
{
    public async Task Handle(UpdateMajorCommand request, CancellationToken cancellationToken)
    {
        // Check if the major exists
        var major = await queryRepository.TableNoTracking
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (major is null)
            throw SmartAttendanceException.NotFound("Major not found");

        // Check for name conflicts under the same HeadMaster
        var conflict = await queryRepository.TableNoTracking
            .AnyAsync(a => a.Id != request.Id &&
                           a.Name.Trim() == request.Name.Trim() &&
                           a.HeadMasterId == request.HeadMasterId,
                cancellationToken);

        if (conflict)
            throw SmartAttendanceException.Conflict("Another major with the same name exists for this headmaster");

        // Map the updated values
        major.Name = request.Name.Trim();
        major.HeadMasterId = request.HeadMasterId;

        // Save changes
        await commandRepository.UpdateAsync(major, cancellationToken);
    }
}