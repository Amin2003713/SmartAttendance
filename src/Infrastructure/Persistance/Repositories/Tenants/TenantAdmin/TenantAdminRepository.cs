using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shifty.Common;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Tenants.Users;
using Shifty.Persistence.Db;
using Shifty.Resources.Messages;
// Added for logging

namespace Shifty.Persistence.Repositories.Tenants.TenantAdmin
{
    public class TenantAdminRepository : ITenantAdminRepository, IScopedDependency
    {
        private readonly TenantDbContext _dbContext;
        private readonly ILogger<TenantAdminRepository> _logger;
        private readonly CompanyMessages _messages;

        private DbSet<Domain.Tenants.TenantAdmin> Entities => _dbContext.Users;
        protected virtual IQueryable<Domain.Tenants.TenantAdmin> Table => Entities;
        protected virtual IQueryable<Domain.Tenants.TenantAdmin> TableNoTracking => Entities.AsNoTracking();

        // Constructor with dependency injection for DbContext and Logger
        public TenantAdminRepository(TenantDbContext dbContext, ILogger<TenantAdminRepository> logger , CompanyMessages messages)
        {
            _dbContext     = dbContext;
            _logger        = logger;
            _messages = messages;
        }

        /// <summary>
        /// Checks if a user exists based on the provided phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number to check.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the user exists; otherwise, false.</returns>
        public async Task<bool> UserExists(string phoneNumber, CancellationToken cancellationToken) =>
            await TableNoTracking.AnyAsync(u => u.PhoneNumber == phoneNumber , cancellationToken);

        /// <summary>
        /// Creates a new TenantAdmin user.
        /// </summary>
        /// <param name="user">The TenantAdmin user to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created TenantAdmin user.</returns>
        public async Task<Domain.Tenants.TenantAdmin> CreateAsync(Domain.Tenants.TenantAdmin user, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await GetByCompanyOrPhoneNumberAsync(user.PhoneNumber, cancellationToken);
                if (existingUser != null)
                {
                    return existingUser;
                }

                Entities.Add(user);
                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("TenantAdmin user with phone number: {PhoneNumber} created successfully.", user.PhoneNumber);
                return user;
            }
            catch (ShiftyException)
            {
                // Re-throw custom exceptions without additional logging
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Source, ex);
                throw  ShiftyException.InternalServerError( additionalData: _messages.Tenant_Admin_Not_Created());
            }
        }

        /// <summary>
        /// Retrieves a TenantAdmin user by phone number. Throws an exception if not found.
        /// </summary>
        /// <param name="phoneNumber">The phone number of the user to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The TenantAdmin user.</returns>
        public async Task<Domain.Tenants.TenantAdmin> GetByCompanyOrPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            try
            {
                return await GetByCompanyOrPhoneNumber(phoneNumber , cancellationToken);
                
                _logger.LogInformation("TenantAdmin user with phone number: {PhoneNumber} retrieved successfully." , phoneNumber);
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Source , ex);
                throw ShiftyException.InternalServerError();
            }
        }

        /// <summary>
        /// Private method to retrieve a TenantAdmin user by phone number without throwing exceptions.
        /// </summary>
        /// <param name="phoneNumber">The phone number of the user to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The TenantAdmin user if found; otherwise, null.</returns>
        private async Task<Domain.Tenants.TenantAdmin> GetByCompanyOrPhoneNumber(string phoneNumber, CancellationToken cancellationToken)
        {
            return await Table.SingleOrDefaultAsync(a => a.PhoneNumber == phoneNumber, cancellationToken);
        }
    }
}
