using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Discounts.Request.Queries.GetAllDiscount;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Tenants.Discounts;

namespace Shifty.Application.Interfaces.Tenants.Discounts;

public interface IDiscountQueryRepository : IScopedDependency
{
    Task<bool>                         DiscountExists(string code, CancellationToken cancellationToken);
    Task<List<GetAllDiscountResponse>> GetAllDiscounts(CancellationToken cancellationToken);
    Task<Discount>                     GetDiscount(Guid id, CancellationToken cancellationToken);
    Task<Discount?>                    GetDiscount(string code, CancellationToken cancellationToken);
    Task<TenantDiscount>               GetTenantDiscounts(Guid discountId, string tenantId, CancellationToken cancellationToken);
}