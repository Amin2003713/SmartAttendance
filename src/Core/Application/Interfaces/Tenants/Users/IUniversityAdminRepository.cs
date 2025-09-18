using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Domain.Tenants;

namespace SmartAttendance.Application.Interfaces.Tenants.Users;

public interface IUniversityAdminRepository
{
    Task<bool> UserExists(string phoneNumber, CancellationToken cancellationToken);

    // Creates a new user asynchronously
    Task<UniversityAdmin> CreateAsync(UniversityAdmin user, CancellationToken cancellationToken);

    Task<UniversityAdmin> GetByUniversityOrPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
}