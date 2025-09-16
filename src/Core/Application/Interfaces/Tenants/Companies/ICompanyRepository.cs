using System.Linq.Expressions;
using SmartAttendance.Application.Base.Companies.Commands.AddRequest;

namespace SmartAttendance.Application.Interfaces.Tenants.Companies;

public interface ICompanyRepository : IScopedDependency
{
    Task<bool>                                   IdentifierExistsAsync(string  identifierId, CancellationToken cancellationToken);
    Task<SmartAttendanceTenantInfo>              GetByIdAsync(string           tenantId,     CancellationToken cancellationToken);
    Task<IEnumerable<SmartAttendanceTenantInfo>> GetAllAsync(CancellationToken cancellationToken);
    Task<SmartAttendanceTenantInfo>              GetByIdentifierAsync(string   code, CancellationToken cancellationToken);

    Task<SmartAttendanceTenantInfo> CreateAsync(
        SmartAttendanceTenantInfo tenantInfo,
        CancellationToken         cancellationToken,
        bool                      saveNow = true);

    Task       CreateAsync(TenantUser tenantUser,   CancellationToken cancellationToken);
    Task<bool> ValidateDomain(string  identifierId, CancellationToken cancellationToken);

    Task<SmartAttendanceTenantInfo> GetEntity(
        Expression<Func<SmartAttendanceTenantInfo, bool>> prediction,
        CancellationToken                                 cancellationToken);

    Task Update(SmartAttendanceTenantInfo company);

    Task<List<TenantUser>> FindByPhoneNumberAsync(string PhoneNumber, CancellationToken cancellationToken);
    Task<TenantUser>       FindByIdAsync(Guid            userId,      CancellationToken cancellationToken);
    Task<List<TenantUser>> FindByUserNameAsync(string    UserName,    CancellationToken cancellationToken);
    Task                   AddRequest(AddRequestCommand  request,     CancellationToken cancellationToken);
}