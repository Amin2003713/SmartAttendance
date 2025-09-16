namespace SmartAttendance.Application.Interfaces.Tenants.Users;

public interface ITenantAdminRepository
{
    Task<bool> UserExists(string phoneNumber, CancellationToken cancellationToken);

    // Creates a new user asynchronously
    Task<TenantAdmin> CreateAsync(TenantAdmin user, CancellationToken cancellationToken);

    Task<TenantAdmin> GetByCompanyOrPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
}