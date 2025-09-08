using Mapster;
using Microsoft.AspNetCore.Identity;
using SmartAttendance.Application.Features.Users.Queries.GetById;
using SmartAttendance.Application.Features.Users.Requests.Queries.GetUserInfo.GetById;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.RequestHandlers.Features.Users.Queries.GetUserInfo.GetById;

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
            SmartAttendanceException.NotFound("User Not Found!");
        }

        logger.LogInformation("User with ID {UserId} retrieved successfully.", request.UserId);
        return result.Adapt<GetUserByIdResponse>();
    }
}