using Mapster;
using Shifty.Application.Base.Storage.Queries.GetAllRemainStorage;
using Shifty.Application.Base.Storage.Request.Queries.GetRemainStorage;
using Shifty.Application.Interfaces.Storages;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Base.Storages.Queries.GetRemainStorage;

public class GetRemainStorageQueryHandler(
    IStorageQueryRepository storageQueryRepository,
    IdentityService service,
    ILogger<GetRemainStorageQueryHandler> logger,
    IMediator mediator,
    IStringLocalizer<GetRemainStorageQueryHandler> localizer
) : IRequestHandler<GetAllRemainStorageQuery, GetRemainStorageResponse>
{
    public async Task<GetRemainStorageResponse> Handle(
        GetAllRemainStorageQuery request,
        CancellationToken cancellationToken)
    {
        // var access = await mediator.Send(new GetProjectUser(service.GetUserId<Guid>()), cancellationToken);
        // todo : handel accesses 

        // if (access == null)
        // {
        //     logger.LogWarning("User {UserId} has no access to any projects.", service.GetUserId());
        //     throw ShiftyException.BadRequest(localizer["You do not have access to any projects."]);
        // }

        var result = await storageQueryRepository.StorageInfo(cancellationToken);

        logger.LogInformation("Remaining storage info retrieved for user {UserId}.", service.GetUserId());

        return result.Adapt<GetRemainStorageResponse>();
    }
}