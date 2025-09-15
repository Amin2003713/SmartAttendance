using Mapster;
using Microsoft.AspNetCore.Identity;
using SmartAttendance.Application.Commons.Queries.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Application.Features.Users.Queries.GetUserInfo.LoggedIn;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Users;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Users.Queries.GetUserInfo.UserInfo;

public class UserInfoQueryHandler(
    UserManager<User>                      userManager,
    IdentityService                        identityService,
    IMediator                              mediator,
    IStringLocalizer<UserInfoQueryHandler> localizer,
    ILogger<UserInfoQueryHandler>          logger
) : IRequestHandler<LoggedInUserInfoQuery, GetUserResponse>
{
    public async Task<GetUserResponse> Handle(LoggedInUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(identityService.GetUserId().ToString()!);

        logger.LogInformation("User info retrieved successfully for current user.");

        if (user == null)
            throw SmartAttendanceException.NotFound(localizer["User not found"].Value);

        var result = user.Adapt<GetUserResponse>();

        if (user.CreatedBy != null)
            result.CreatedBy =
                await mediator.Send(new GetLogPropertyInfoQuery(user.CreatedBy.Value), cancellationToken);

        result.Roles = (await userManager.GetRolesAsync(user) as List<string>)!.Select(a => a.ToLower()).ToList();


        return result;
    }
}