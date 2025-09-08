using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Tenants;

namespace SmartAttendance.Application.Interfaces.Tenants.Prices;

public interface IPriceCommandRepository : IScopedDependency
{
    Task CreateNewPrice(Price price, CancellationToken cancellationToken);
}