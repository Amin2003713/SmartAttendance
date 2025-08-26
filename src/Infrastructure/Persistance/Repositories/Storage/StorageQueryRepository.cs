using Shifty.Application.Base.Payment.Queries.GetActivePayment;
using Shifty.Application.Base.Storage.Request.Queries.GetRemainStorageByProject;
using Shifty.Application.Interfaces.Storages;

namespace Shifty.Persistence.Repositories.Storage;

public class StorageQueryRepository(
    ReadOnlyDbContext dbContext,
    IMediator mediator,
    ILogger<QueryRepository<Domain.Storages.Storage>> logger
)
    : QueryRepository<Domain.Storages.Storage>(dbContext, logger),
        IStorageQueryRepository
{
    // public async Task<GetRemainStorageResponse> StorageInfo(CancellationToken cancellationToken)
    // {
    //     try
    //     {
    //         var payment =
    //             await mediator.Send(new GetActivePaymentQuery(), cancellationToken);
    //
    //         if (payment == null)
    //             // Handle the case where the purchase record is missing
    //             return new GetRemainStorageResponse(0, 0, 0, false);
    //
    //         var grantedSpace = payment.GrantedStorageMb;
    //
    //         try
    //         {
    //             var storages = await TableNoTracking.Where(a => a.IsActive)
    //                 .SumAsync(a => a.StorageUsedByItemMb, cancellationToken);
    //
    //             if (storages is 0)
    //                 return new GetRemainStorageResponse(grantedSpace, 0, grantedSpace, false);
    //
    //             var available = grantedSpace - storages;
    //
    //             return new GetRemainStorageResponse(grantedSpace, storages, available, available <= 100);
    //         }
    //         catch (Exception e)
    //         {
    //             return new GetRemainStorageResponse(grantedSpace, 0, grantedSpace, false);
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         return new GetRemainStorageResponse(0, 0, 0, false);
    //     }
    // }

    public async Task<GetRemainStorageByProjectResponse> StorageInfo(
        CancellationToken cancellationToken)
    {
        var payment =
            await mediator.Send(new GetActivePaymentQuery(), cancellationToken);


        if (payment == null)
            // Handle the case where the purchase record is missing
            return new GetRemainStorageByProjectResponse(0, 0, 0, false);

        var grantedSpace = payment.GrantedStorageMb;

        try
        {
            var storages = await TableNoTracking.Where(a => a.IsActive
                                                         // && a.ProjectId == projectId
                                                            )
                .SumAsync(a => a.StorageUsedByItemMb, cancellationToken);

            if (storages is 0)
                return new GetRemainStorageByProjectResponse(grantedSpace, 0, grantedSpace, false);

            var available = grantedSpace - storages;

            return new GetRemainStorageByProjectResponse(grantedSpace, storages, available, available <= 100);
        }
        catch (Exception e)
        {
            return new GetRemainStorageByProjectResponse(grantedSpace, 0, grantedSpace, false);
        }
    }
}