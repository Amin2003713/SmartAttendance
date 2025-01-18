using Shifty.Domain.Interfaces.Base;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Domain.Interfaces.Users
{
    public interface ITenantAdminRepository 
    {
     

        Task<bool> UserExists(string phoneNumber , CancellationToken cancellationToken);

        // Creates a new user asynchronously
        Task<TenantAdmin> CreateAsync(TenantAdmin user , CancellationToken cancellationToken);

        Task<TenantAdmin> GetByCompanyOrPhoneNumberAsync(string phoneNumber , CancellationToken cancellationToken);

    }
}