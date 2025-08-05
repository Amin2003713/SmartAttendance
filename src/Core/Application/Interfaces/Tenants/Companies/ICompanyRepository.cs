using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Companies.Commands.AddRequest;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Tenants;

namespace Shifty.Application.Interfaces.Tenants.Companies;

public interface ICompanyRepository : IScopedDependency
{
    Task<bool>                          IdentifierExistsAsync(string identifierId, CancellationToken cancellationToken);
    Task<ShiftyTenantInfo>              GetByIdAsync(string tenantId, CancellationToken cancellationToken);
    Task<IEnumerable<ShiftyTenantInfo>> GetAllAsync(CancellationToken cancellationToken);
    Task<ShiftyTenantInfo>              GetByIdentifierAsync(string code, CancellationToken cancellationToken);
    Task<ShiftyTenantInfo>              CreateAsync(ShiftyTenantInfo tenantInfo, CancellationToken cancellationToken, bool saveNow = true);
    Task                                CreateAsync(TenantUser tenantUser, CancellationToken cancellationToken);
    Task<bool>                          ValidateDomain(string identifierId, CancellationToken cancellationToken);

    Task<ShiftyTenantInfo> GetEntity(
        Expression<Func<ShiftyTenantInfo, bool>> prediction,
        CancellationToken cancellationToken);

    Task Update(ShiftyTenantInfo company);

    Task<List<TenantUser>> FindByPhoneNumberAsync(string PhoneNumber, CancellationToken cancellationToken);
    Task<TenantUser>       FindByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<List<TenantUser>> FindByUserNameAsync(string UserName, CancellationToken cancellationToken);
    Task                   AddRequest(AddRequestCommand request, CancellationToken cancellationToken);
}