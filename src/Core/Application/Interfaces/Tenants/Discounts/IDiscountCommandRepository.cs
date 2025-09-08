using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Tenants.Discounts;

namespace SmartAttendance.Application.Interfaces.Tenants.Discounts;

public interface IDiscountCommandRepository : IScopedDependency
{
    Task Add(Discount discount, CancellationToken cancellationToken);
    Task Delete(Discount discount, CancellationToken cancellationToken);
    Task UpdateTenantDiscount(TenantDiscount tenantDiscount, CancellationToken cancellationToken);
}