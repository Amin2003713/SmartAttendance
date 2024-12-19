using Shifty.Domain.Interfaces.Base;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Domain.Interfaces.Users
{
    public interface ITenantAdminRepository 
    {
     

        Task<bool> UserExists(string username, CancellationToken cancellationToken);

        // Creates a new user asynchronously
        Task<bool> CreateAsync(TenantAdmin user, string password, CancellationToken cancellationToken);

        // Retrieves a user by username
        Task<TenantAdmin> GetByUsernameAsync(string username, CancellationToken cancellationToken);

        // Updates the password of an existing user
        Task UpdatePasswordAsync(string username, string newPassword, CancellationToken cancellationToken);

        // Validates the password for a given username
        bool ValidatePasswordAsync(TenantAdmin user, string password, CancellationToken cancellationToken);

        // Updates an existing user's details
        Task UpdateUserAsync(TenantAdmin user, CancellationToken cancellationToken);

        // Deletes a user by username
        Task DeleteUserAsync(string username, CancellationToken cancellationToken);



        // Task AddOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        // Task<bool> ValidateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    }
}