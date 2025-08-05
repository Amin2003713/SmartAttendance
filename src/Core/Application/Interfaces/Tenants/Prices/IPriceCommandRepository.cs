using System.Threading;
using System.Threading.Tasks;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Tenants;

namespace Shifty.Application.Interfaces.Tenants.Prices;

public interface IPriceCommandRepository : IScopedDependency
{
    Task CreateNewPrice(Price price, CancellationToken cancellationToken);
}