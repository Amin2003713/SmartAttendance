using Microsoft.AspNetCore.Identity;
using Shifty.Application.Users.Commands.DeleteRole;
using Shifty.Common.Exceptions;
using Shifty.Domain.Users;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Users.Commands.DeleteRole;

public class DeleteRoleCommandHandler(
    IdentityService service,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    ILogger<DeleteRoleCommandHandler> logger,
    IStringLocalizer<DeleteRoleCommandHandler> localizer
) : IRequestHandler<DeleteRoleCommand>
{
    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var userId = service.GetUserId<Guid>();
        var user   = await userManager.FindByIdAsync(request.UserId.ToString());

        if (userId == request.UserId) throw IpaException.BadRequest(localizer["You cannot remove your own roles."]);

        if (user == null)
        {
            logger.LogWarning("User with ID {UserId} not found.", request.UserId);
            throw IpaException.NotFound(localizer["User not found."]);
        }

        var currentRoles = await userManager.GetRolesAsync(user);

        if (!currentRoles.Contains(request.Role))
        {
            logger.LogWarning("User {UserId} does not have role {RoleName}.", request.UserId, request.Role);
            throw IpaException.BadRequest(localizer["User does not have the specified role."]);
        }

        if (!await roleManager.RoleExistsAsync(request.Role))
        {
            logger.LogWarning("Role {RoleName} does not exist.", request.Role);
            throw IpaException.BadRequest(localizer["Role does not exist."]);
        }

        var result = await userManager.RemoveFromRoleAsync(user, request.Role);

        if (!result.Succeeded)
        {
            logger.LogError("Failed to remove role {RoleName} from user {UserId}.", request.Role, request.UserId);
            throw IpaException.BadRequest(localizer["Failed to remove role."]);
        }

        logger.LogInformation("Role {RoleName} removed from user {UserId}.", request.Role, request.UserId);
    }
}