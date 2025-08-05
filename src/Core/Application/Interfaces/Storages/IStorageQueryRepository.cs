using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Interfaces.Base;
using Shifty.Application.Storage.Request.Queries.GetRemainStorage;
using Shifty.Application.Storage.Request.Queries.GetRemainStorageByProject;
using Shifty.Common.Utilities.InjectionHelpers;

namespace Shifty.Application.Interfaces.Storages;

public interface IStorageQueryRepository : IQueryRepository<Domain.Storages.Storage>,
    IScopedDependency
{
    // Task<GetRemainStorageResponse> StorageInfo(CancellationToken cancellationToken);

    Task<GetRemainStorageByProjectResponse> StorageInfo( CancellationToken cancellationToken);
}