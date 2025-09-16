using Microsoft.AspNetCore.Identity;
using SmartAttendance.Application.Features.Users.Commands.AddRole;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.Utilities.EnumHelpers;
using SmartAttendance.Common.Utilities.RolesHelper;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.RequestHandlers.Features.Users.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler(
    UserManager<User>                              userManager,
    RoleManager<Role>                              roleManager,
    ILogger<UpdateEmployeeCommandHandler>          logger,
    IStringLocalizer<UpdateEmployeeCommandHandler> localizer
) : IRequestHandler<UpdateEmployeeCommand>
{
    public async Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // 1. Parse and validate requested roles
        var requestedRoles = RoleParser.ParseMany(request.Roles).ToHashSet();

        // 2. Find user
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
        {
            logger.LogWarning("User with ID {UserId} not found.", request.UserId);
            throw SmartAttendanceException.NotFound(localizer["User not found."]);
        }

        // 3. Get current roles
        var currentRoleNames = await userManager.GetRolesAsync(user);
        var currentRoles     = currentRoleNames.Where(RoleParser.IsValid).Select(RoleParser.Parse).ToHashSet();

        // 4. Determine roles to add
        var rolesToAdd = requestedRoles.Except(currentRoles).SelectMany(r => r.GetAllAdditiveRoles()).ToHashSet();

        // 5. Determine roles to remove
        var baseRemovals  = currentRoles.Except(requestedRoles);
        var rolesToRemove = baseRemovals.SelectMany(r => r.GetAllRemovableRoles()).ToHashSet();

        // 6. Ensure all roles to be added exist
        await EnsureRolesExist(rolesToAdd);

        // 7. Add roles
        foreach (var role in rolesToAdd)
        {
            var roleName = role.GetEnglishName();
            var result   = await userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                logger.LogError("Failed to assign role {RoleTypes} to user {UserId}", roleName, request.UserId);
                throw SmartAttendanceException.BadRequest(localizer["Failed to assign role {0}.", roleName]);
            }
        }

        // 8. Remove roles
        foreach (var role in rolesToRemove)
        {
            var roleName = role.GetEnglishName();
            if (!await userManager.IsInRoleAsync(user, roleName)) continue;

            var result = await userManager.RemoveFromRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                logger.LogError("Failed to remove role {RoleTypes} from user {UserId}", roleName, request.UserId);
                throw SmartAttendanceException.BadRequest(localizer["Failed to remove role {0}.", roleName]);
            }
        }

        logger.LogInformation("Updated roles for user {UserId}", request.UserId);
    }

    private async Task EnsureRolesExist(IEnumerable<RolesType> roles)
    {
        foreach (var role in roles)
        {
            var roleName = role.GetEnglishName();

            if (await roleManager.RoleExistsAsync(roleName)) continue;

            var result = await roleManager.CreateAsync(new Role
            {
                Name        = roleName,
                Description = roleName
            });

            if (!result.Succeeded)
            {
                logger.LogError("Failed to create missing role: {RoleTypes}", roleName);
                throw SmartAttendanceException.BadRequest(localizer["Failed to create role {0}.", roleName]);
            }
        }
    }
}