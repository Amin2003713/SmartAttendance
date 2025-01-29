using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Domain.Tenants;

namespace Shifty.Domain.Interfaces.Tenants.Companies
{
    public interface ICompanyRepository
    {
        Task<bool> IdentifierExistsAsync(string identifierId , CancellationToken cancellationToken);
        Task<ShiftyTenantInfo> GetByIdAsync(string tenantId , CancellationToken cancellationToken);
        Task<IEnumerable<ShiftyTenantInfo>> GetAllAsync(CancellationToken cancellationToken);
        Task<ShiftyTenantInfo> GetByIdentifierAsync(string code , CancellationToken cancellationToken);
        Task<ShiftyTenantInfo> CreateAsync(ShiftyTenantInfo tenantInfo , CancellationToken cancellationToken , bool saveNow = true);
        Task<bool> ValidateDomain(string identifierId , CancellationToken cancellationToken);
    }
}