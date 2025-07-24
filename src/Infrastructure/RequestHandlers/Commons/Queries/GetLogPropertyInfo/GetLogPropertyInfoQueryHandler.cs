using Microsoft.AspNetCore.Identity;
using Shifty.Application.Commons.Queries.GetLogPropertyInfo.OperatorLogs;
using Shifty.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using Shifty.Common.Exceptions;
using Shifty.Domain.Users;

namespace Shifty.RequestHandlers.Commons.Queries.GetLogPropertyInfo;

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
            throw IpaException.NotFound("Creator was not found");
        }

        logger.LogInformation("User with Id {UserId} found: {UserName}", request.Id, user.FullName());
        return new LogPropertyInfoResponse
        {
            Id = Guid.Empty,
            FirstName = "Aghdas",
            LastName = "nosrat",
            PhoneNumber = "09134041709",
            Profile = "https://example.com/Profil.png"
        };
    }
}