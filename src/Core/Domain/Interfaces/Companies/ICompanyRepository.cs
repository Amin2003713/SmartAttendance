using Shifty.Domain.Tenants;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Domain.Interfaces.Companies
{
    public interface ICompanyRepository
    {
        Task<bool> IdentifierExistsAsync(string identifierId , CancellationToken cancellationToken);
        Task<ShiftyTenantInfo> GetByIdAsync(string tenantId , CancellationToken cancellationToken);
        Task<IEnumerable<ShiftyTenantInfo>> GetAllAsync(CancellationToken cancellationToken);
        Task<ShiftyTenantInfo> GetByIdentifierAsync(string code , CancellationToken cancellationToken);
        Task<ShiftyTenantInfo> CreateAsync(ShiftyTenantInfo tenantInfo , CancellationToken cancellationToken , bool saveNow = true);
        Task<(bool doseExist ,  string message)> ValidateDomain(string identifierId , CancellationToken cancellationToken);
    }
}