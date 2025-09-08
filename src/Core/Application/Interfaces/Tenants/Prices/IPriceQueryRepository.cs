using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Tenants;

namespace SmartAttendance.Application.Interfaces.Tenants.Prices;

public interface IPriceQueryRepository : IScopedDependency
{
    Task<Price> GetPrice(CancellationToken cancellationToken);

    Task<Price> GetPriceById(Guid? id, CancellationToken cancellationToken = default);
}