using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Users.Queries.GetByIds;
using SmartAttendance.Application.Features.Users.Queries.GetNameById;
using SmartAttendance.Application.Interfaces.Users;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.RequestHandlers.Features.Users.Queries.GetNameById;

public class GetNameByIdQueryHandler(
    UserManager<User>                  userManager,
    IStringLocalizer<GetNameByIdQuery> localizer,
    ILogger<GetNameByIdQueryHandler>   logger
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
                SmartAttendanceException.NotFound(localizer["User Not Found."]);
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

public class GetTeacherByIdsHandler(
    IUserQueryRepository queryRepository,
    IStringLocalizer<GetTeacherByIds> localizer,
    ILogger<GetTeacherByIdsHandler>   logger
) : IRequestHandler<GetTeacherByIds, List<GetUserResponse>>
{
    public async Task<List<GetUserResponse>> Handle(GetTeacherByIds request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var result = await queryRepository.TableNoTracking.Where(a => request.Ids.Contains(a.Id)).ToListAsync(cancellationToken);
            return result.Select(a => a.Adapt<GetUserResponse>()).ToList();
        }
        catch (Exception e)
        {
            throw;
        }
    }
}