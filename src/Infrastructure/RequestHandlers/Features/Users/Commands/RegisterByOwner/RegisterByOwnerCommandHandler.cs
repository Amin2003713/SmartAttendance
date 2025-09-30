using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Users.Commands.RegisterByOwner;
using SmartAttendance.Application.Interfaces.Users;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.Utilities.EnumHelpers;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.RequestHandlers.Features.Users.Commands.RegisterByOwner;

public class RegisterByOwnerCommandHandler(
    IUserCommandRepository                          commandRepository,
    IUserQueryRepository                            queryRepository,
    ILogger<RegisterByOwnerCommandHandler>          logger,
    RoleManager<IdentityRole<Guid>> RoleManager,
    UserManager<User> _userManager,
    IStringLocalizer<RegisterByOwnerCommandHandler> localizer
) : IRequestHandler<RegisterByOwnerCommand>
{
    public async Task Handle(RegisterByOwnerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await queryRepository.TableNoTracking.AnyAsync(a => a.PhoneNumber == request.PhoneNumber,
                    cancellationToken))
            {
                logger.LogWarning("Duplicate phone number detected: {PhoneNumber}", request.PhoneNumber);
                throw SmartAttendanceException.BadRequest(localizer["This phone number is already registered."].Value);
            }

            var user = await commandRepository.RegisterByOwnerAsync(request, cancellationToken);


            if (!await RoleManager.RoleExistsAsync(request.Role.GetEnglishName().ToLower()))
            {
                // Optionally, create the role if it doesn't exist
                var roleResult = await RoleManager.CreateAsync(new IdentityRole<Guid>(request.Role.GetEnglishName().ToLower()));
                if (!roleResult.Succeeded)
                    throw SmartAttendanceException.InternalServerError(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }


            var addResult = await _userManager.AddToRolesAsync(user, [request.Role.GetEnglishName().ToLower()]);
            if (!addResult.Succeeded)
                throw SmartAttendanceException.InternalServerError(string.Join(", ", addResult.Errors.Select(e => e.Description)));


            logger.LogInformation("User with phone number {PhoneNumber} registered by owner successfully.",
                request.PhoneNumber);
        }
        catch (Exception e)
        {
            logger.LogError(e.Source, e);
            throw;
        }
    }
}