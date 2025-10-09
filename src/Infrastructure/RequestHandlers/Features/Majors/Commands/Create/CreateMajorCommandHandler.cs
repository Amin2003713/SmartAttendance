using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Majors.Commands.Create;
using SmartAttendance.Application.Interfaces.Majors;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Domain.Features.Majors;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.RequestHandlers.Features.Majors.Commands.Create;

public class CreateMajorCommandHandler(
    IMajorQueryRepository queryRepository ,
    IMajorCommandRepository commandRepository  ,
    UserManager<User> userManager
)  : IRequestHandler<CreateMajorCommand>
{
    public async Task Handle(CreateMajorCommand request, CancellationToken cancellationToken)
    {
        if (await queryRepository.TableNoTracking.AnyAsync(a => a.Name == request.Name && a.HeadMasterId == request.HeadMasterId,
                cancellationToken: cancellationToken))
            throw SmartAttendanceException.Conflict();

        var user = await userManager.FindByIdAsync(request.HeadMasterId.ToString() ?? string.Empty);

        if (user == null || !(await userManager.GetRolesAsync(user))[0].Equals(nameof(Roles.HeadMaster) , StringComparison.OrdinalIgnoreCase))
            throw SmartAttendanceException.Forbidden("fob");

        await commandRepository.AddAsync(request.Adapt<Major>() , cancellationToken);
    }
}