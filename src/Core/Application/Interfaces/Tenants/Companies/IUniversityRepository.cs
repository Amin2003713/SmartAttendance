using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Base.Universities.Commands.AddRequest;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Tenants;

namespace SmartAttendance.Application.Interfaces.Tenants.Companies;

public interface IUniversityRepository : IScopedDependency
{
    Task<bool> IdentifierExistsAsync(string identifierId, CancellationToken cancellationToken);

    Task<UniversityTenantInfo>              GetByIdAsync(string tenantId, CancellationToken cancellationToken);
    Task<IEnumerable<UniversityTenantInfo>> GetAllAsync(CancellationToken cancellationToken);
    Task<UniversityTenantInfo>              GetByIdentifierAsync(string code, CancellationToken cancellationToken);

    Task<UniversityTenantInfo> CreateAsync(
        UniversityTenantInfo tenantInfo,
        CancellationToken cancellationToken,
        bool saveNow = true);

    Task CreateAsync(UniversityUser UniversityUser, CancellationToken cancellationToken);

    Task<bool> ValidateDomain(string identifierId, CancellationToken cancellationToken);

    Task<UniversityTenantInfo> GetEntity(
        Expression<Func<UniversityTenantInfo, bool>> prediction,
        CancellationToken cancellationToken);

    Task Update(UniversityTenantInfo university);

    Task<List<UniversityUser>> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<UniversityUser?>      FindByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<List<UniversityUser>> FindByUserNameAsync(string userName, CancellationToken cancellationToken);

    Task AddRequest(AddRequestCommand request, CancellationToken cancellationToken);
}