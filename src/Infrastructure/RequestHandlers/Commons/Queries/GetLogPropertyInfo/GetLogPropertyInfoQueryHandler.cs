using Microsoft.AspNetCore.Identity;
using SmartAttendance.Application.Commons.Queries.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.RequestHandlers.Commons.Queries.GetLogPropertyInfo;

public class GetLogPropertyInfoQueryHandler(
    UserManager<User> userManager,
    ILogger<GetLogPropertyInfoQueryHandler> logger
)
    : IRequestHandler<GetLogPropertyInfoQuery, LogPropertyInfoResponse>
{
    public async Task<LogPropertyInfoResponse> Handle(
        GetLogPropertyInfoQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching log property info for UserId: {UserId}", request.Id);

        var user = await userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
        {
            logger.LogWarning("User with Id {UserId} not found.", request.Id);
            throw SmartAttendanceException.NotFound("Creator was not found");
        }

        logger.LogInformation("User with Id {UserId} found: {UserName}", request.Id, user.FullName());
        return new LogPropertyInfoResponse
        {
            Id = Guid.Empty,
            Name = "Aghdas",
            Profile = "https://example.com/Profil.png"
        };
    }
}