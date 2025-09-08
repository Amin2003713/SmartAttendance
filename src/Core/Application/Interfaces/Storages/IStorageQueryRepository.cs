using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Base.Storage.Request.Queries.GetRemainStorageByProject;
using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Storages;

namespace SmartAttendance.Application.Interfaces.Storages;

public interface IStorageQueryRepository : IQueryRepository<Storage>,
    IScopedDependency
{
    // Task<GetRemainStorageResponse> StorageInfo(CancellationToken cancellationToken);

    Task<GetRemainStorageByProjectResponse> StorageInfo( CancellationToken cancellationToken);
}