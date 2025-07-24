using Mapster;
using Microsoft.AspNetCore.Identity;
using Shifty.Application.Users.Queries.GetById;
using Shifty.Application.Users.Requests.Queries.GetUserInfo.GetById;
using Shifty.Common.Exceptions;
using Shifty.Domain.Users;

namespace Shifty.RequestHandlers.Users.Queries.GetUserInfo.GetById;

public class GetUserByIdQueryHandler(
    UserManager<User> userManager,
    ILogger<GetUserByIdQueryHandler> logger
) : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await userManager.FindByIdAsync(request.UserId.ToString());

        if (result == null)
        {
            logger.LogWarning("User with ID {UserId} not found.", request.UserId);
            IpaException.NotFound("User Not Found!");
        }

        logger.LogInformation("User with ID {UserId} retrieved successfully.", request.UserId);
        return result.Adapt<GetUserByIdResponse>();
    }
}