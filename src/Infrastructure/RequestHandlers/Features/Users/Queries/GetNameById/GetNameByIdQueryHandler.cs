using Microsoft.AspNetCore.Identity;
using Shifty.Application.Features.Users.Queries.GetNameById;
using Shifty.Common.Exceptions;
using Shifty.Domain.Users;

namespace Shifty.RequestHandlers.Features.Users.Queries.GetNameById;

public class GetNameByIdQueryHandler(
    UserManager<User> userManager,
    IStringLocalizer<GetNameByIdQuery> localizer,
    ILogger<GetNameByIdQueryHandler> logger
) : IRequestHandler<GetNameByIdQuery, string>
{
    public async Task<string> Handle(GetNameByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var user = await userManager.FindByIdAsync(request.Id.ToString());

            if (user == null)
            {
                logger.LogWarning("User with ID {UserId} not found.", request.Id);
                IpaException.NotFound(localizer["User Not Found."]);
            }

            var fullName = user!.FullName() ?? "";
            logger.LogInformation("Full name retrieved for user {UserId}: {FullName}", request.Id, fullName);

            return fullName;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error occurred while retrieving user name for ID {UserId}.", request.Id);
            throw;
        }
    }
}