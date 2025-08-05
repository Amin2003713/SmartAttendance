using System.Threading;
using System.Threading.Tasks;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Tenants;

namespace Shifty.Application.Interfaces.Tenants.Prices;

public interface IPriceQueryRepository : IScopedDependency
{
    Task<Price> GetPrice(CancellationToken cancellationToken);

    Task<Price> GetPriceById(Guid? id, CancellationToken cancellationToken = default);
}