using Mapster;
using Shifty.Application.Base.Storage.Queries.GetRemainStorageByProject;
using Shifty.Application.Base.Storage.Request.Queries.GetRemainStorageByProject;
using Shifty.Application.Interfaces.Storages;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Base.Storages.Queries.GetRemainStorageByProject;

public class GetRemainStorageByProjectQueryHandler(
    IStorageQueryRepository storageQueryRepository,
    IdentityService service,
    ILogger<GetRemainStorageByProjectQueryHandler> logger,
    IStringLocalizer<GetRemainStorageByProjectQueryHandler> localizer
) : IRequestHandler<GetRemainStorageByProjectQuery, GetRemainStorageByProjectResponse>
{
    public async Task<GetRemainStorageByProjectResponse> Handle(
        GetRemainStorageByProjectQuery request,
        CancellationToken cancellationToken)
    {
        var userId = service.GetUserId<Guid>();

        // var access = await broker
        //     .RequestAsync<GetProjectUserAccessBrokerResponse, GetProjectUserAccessBroker>(new GetProjectUserAccessBroker(request.ProjectId!.Value, userId),
        //         cancellationToken);

        // if (access == null)
        // {
        //     logger.LogWarning("User {UserId} has no access to any projects.", userId);
        //     throw ShiftyException.BadRequest(localizer["You do not have access to any projects."]);
        // }

        var result = await storageQueryRepository.StorageInfo(cancellationToken);

        // logger.LogInformation("Remaining storage retrieved for project {ProjectId} by user {UserId}.",
        //     request.ProjectId,
        //     userId);

        return result.Adapt<GetRemainStorageByProjectResponse>();
    }
}