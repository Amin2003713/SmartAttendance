using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shifty.Common;
using Shifty.Domain.Interfaces.Users;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.Repositories.Users
{
    public class TenantAdminRepository(TenantDbContext dbContext , IPasswordHasher<TenantAdmin> passwordHasher) : Repository<TenantAdmin, TenantDbContext>(dbContext), ITenantAdminRepository, IScopedDependency
    {
        public async Task<bool> UserExists(string username, CancellationToken cancellationToken) =>
            await TableNoTracking.AnyAsync(u => u.UserName == username, cancellationToken);


        public async Task<bool> CreateAsync(TenantAdmin user, string password, CancellationToken cancellationToken)
        {
            try
            {
                user.SetUserName();
                if (await UserExists(user.UserName, cancellationToken))
                    throw new ShiftyException(ApiResultStatusCode.BadRequest, "User already exists.");
                // Hash the user's password before saving
                user.PasswordHash = passwordHasher.HashPassword(user, password!);

                // Save the user to the database
                await Entities.AddAsync(user, cancellationToken);
                await DbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<TenantAdmin> GetByUsernameAsync(string username, CancellationToken cancellationToken) =>
            await TableNoTracking.FirstOrDefaultAsync(u => u.UserName == username, cancellationToken: cancellationToken);

        public async Task UpdatePasswordAsync(string username, string newPassword, CancellationToken cancellationToken)
        {
            var user = await GetByUsernameAsync(username , cancellationToken);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Hash the new password
            user.PasswordHash = passwordHasher.HashPassword(user, newPassword);

            // Save the updated user
            Entities.Update(user);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public bool ValidatePasswordAsync(TenantAdmin user, string password, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ShiftyException(ApiResultStatusCode.NotFound , "User not found");

            // Validate the provided password against the stored hash
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);
            return result == PasswordVerificationResult.Success;
        }

        // Additional methods you may need
        public async Task UpdateUserAsync(TenantAdmin user, CancellationToken cancellationToken)
        {
            Entities.Update(user);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUserAsync(string username, CancellationToken cancellationToken)
        {
            var user = await GetByUsernameAsync(username , cancellationToken);
            if (user != null)
            {
                Entities.Remove(user);
                await DbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}