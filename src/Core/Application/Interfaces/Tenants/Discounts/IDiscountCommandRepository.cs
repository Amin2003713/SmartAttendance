using System.Threading;
using System.Threading.Tasks;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Tenants.Discounts;

namespace Shifty.Application.Interfaces.Tenants.Discounts;

public interface IDiscountCommandRepository : IScopedDependency
{
    Task Add(Discount discount, CancellationToken cancellationToken);
    Task Delete(Discount discount, CancellationToken cancellationToken);
    Task UpdateTenantDiscount(TenantDiscount tenantDiscount, CancellationToken cancellationToken);
}