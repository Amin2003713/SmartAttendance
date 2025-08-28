using Mapster;
using Microsoft.AspNetCore.Identity;
using Shifty.Application.Commons.Queries.GetLogPropertyInfo.OperatorLogs;
using Shifty.Application.Features.Users.Queries.GetUserInfo.LoggedIn;
using Shifty.Common.Common.Responses.Users.Queries.Base;
using Shifty.Common.Exceptions;
using Shifty.Domain.Users;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.Users.Queries.GetUserInfo.UserInfo;

public class UserInfoQueryHandler(
    UserManager<User> userManager,
    IdentityService identityService,
    IMediator mediator,
    IStringLocalizer<UserInfoQueryHandler> localizer,
    ILogger<UserInfoQueryHandler> logger
) : IRequestHandler<LoggedInUserInfoQuery, GetUserResponse>
{
    public async Task<GetUserResponse> Handle(LoggedInUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(identityService.GetUserId().ToString()!);

        logger.LogInformation("User info retrieved successfully for current user.");

        if (user == null)
            throw ShiftyException.NotFound(localizer["User not found"].Value);

        var result = user.Adapt<GetUserResponse>();

        if (user.CreatedBy != null)
            result.CreatedBy =
                await mediator.Send(new GetLogPropertyInfoQuery(user.CreatedBy.Value), cancellationToken);

        result.Roles = (await userManager.GetRolesAsync(user) as List<string>)!.Select(a => a.ToLower()).ToList();


        return result;
    }
}