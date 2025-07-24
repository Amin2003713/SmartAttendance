using Microsoft.AspNetCore.Identity;
using Shifty.Application.Users.Commands.AddRole;
using Shifty.Common.Exceptions;
using Shifty.Common.General.Enums;
using Shifty.Common.Utilities.EnumHelpers;
using Shifty.Common.Utilities.RolesHelper;
using Shifty.Domain.Users;

namespace Shifty.RequestHandlers.Users.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    ILogger<UpdateEmployeeCommandHandler> logger,
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
            throw IpaException.NotFound(localizer["User not found."]);
        }

        // 3. Get current roles
        var currentRoleNames = await userManager.GetRolesAsync(user);
        var currentRoles = currentRoleNames
            .Where(RoleParser.IsValid)
            .Select(RoleParser.Parse)
            .ToHashSet();

        // 4. Determine roles to add
        var rolesToAdd = requestedRoles
            .Except(currentRoles)
            .SelectMany(r => r.GetAllAdditiveRoles())
            .ToHashSet();

        // 5. Determine roles to remove
        var baseRemovals = currentRoles.Except(requestedRoles);
        var rolesToRemove = baseRemovals
            .SelectMany(r => r.GetAllRemovableRoles())
            .ToHashSet();

        // 6. Ensure all roles to be added exist
        await EnsureRolesExist(rolesToAdd);

        // 7. Add roles
        foreach (var role in rolesToAdd)
        {
            var roleName = role.GetEnglishName();
            var result   = await userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                logger.LogError("Failed to assign role {Role} to user {UserId}", roleName, request.UserId);
                throw IpaException.BadRequest(localizer["Failed to assign role {0}.", roleName]);
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
                logger.LogError("Failed to remove role {Role} from user {UserId}", roleName, request.UserId);
                throw IpaException.BadRequest(localizer["Failed to remove role {0}.", roleName]);
            }
        }

        logger.LogInformation("Updated roles for user {UserId}", request.UserId);
    }

    private async Task EnsureRolesExist(IEnumerable<Roles> roles)
    {
        foreach (var role in roles)
        {
            var roleName = role.GetEnglishName();

            if (await roleManager.RoleExistsAsync(roleName)) continue;

            var result = await roleManager.CreateAsync(new Role
            {
                Name = roleName, Description = roleName
            });

            if (!result.Succeeded)
            {
                logger.LogError("Failed to create missing role: {Role}", roleName);
                throw IpaException.BadRequest(localizer["Failed to create role {0}.", roleName]);
            }
        }
    }
}