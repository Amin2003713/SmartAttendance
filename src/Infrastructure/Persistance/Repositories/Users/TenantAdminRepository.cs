using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shifty.Common;
using Shifty.Domain.Interfaces.Base;
using Shifty.Domain.Interfaces.Users;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.Repositories.Users
{
    public class TenantAdminRepository(TenantDbContext dbContext , IPasswordHasher<TenantAdmin> passwordHasher) : ITenantAdminRepository, IScopedDependency
    {
        private DbSet<TenantAdmin> Entities { get; } = dbContext.Users;
        protected virtual IQueryable<TenantAdmin> Table => Entities;
        protected virtual IQueryable<TenantAdmin> TableNoTracking => Entities.AsNoTracking();

        public async Task<bool> UserExists(string username, CancellationToken cancellationToken) =>
            await TableNoTracking.AnyAsync(u => u.UserName == username, cancellationToken);

   

        public async Task<bool> CreateAsync(TenantAdmin user, string password, CancellationToken cancellationToken)
        {
            try
            {
                user.SetUserName();
                user.SetIdentityFields();
                if (await UserExists(user.UserName, cancellationToken))
                    throw new ShiftyException(ApiResultStatusCode.BadRequest, "User already exists.");
                // Hash the user's password before saving
                user.PasswordHash = passwordHasher.HashPassword(user, password!);

                // Save the user to the database
                await Entities.AddAsync(user, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<TenantAdmin> GetByUsernameAsync(string username, CancellationToken cancellationToken) =>
            await TableNoTracking.FirstOrDefaultAsync(u => u.UserName == username, cancellationToken: cancellationToken);

        public async Task<TenantAdmin> GetByIdeAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await TableNoTracking.SingleOrDefaultAsync(a=>a.Id == id, cancellationToken);

            return user ?? throw new ShiftyException(ApiResultStatusCode.NotFound, "User does not exist.");
        }

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
            await dbContext.SaveChangesAsync(cancellationToken);
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
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUserAsync(string username, CancellationToken cancellationToken)
        {
            var user = await GetByUsernameAsync(username , cancellationToken);
            if (user != null)
            {
                Entities.Remove(user);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateLastLoginDateAsync(TenantAdmin user, CancellationToken httpContextRequestAborted)
        {
            user.LastLoginDate = DateTime.Now;
            await UpdateUserAsync(user, httpContextRequestAborted); 
        }


        // public async Task AddOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        // {
        //     var token = await refreshRepository.TableNoTracking.SingleOrDefaultAsync(x => x.UserId == refreshToken.UserId, cancellationToken);
        //     if (token == null)
        //     {
        //         refreshToken.CreatedAt = DateTime.Now;
        //         await refreshRepository.AddAsync(refreshToken, cancellationToken);
        //     }
        //     else
        //     {
        //         refreshToken.CreatedAt = token.CreatedAt;
        //         refreshToken.Id = token.Id;
        //         await refreshRepository.UpdateAsync(refreshToken, cancellationToken);
        //     }
        // }
        //
        // public async Task<bool> ValidateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        // {
        //     var result = await refreshRepository.TableNoTracking.SingleOrDefaultAsync(x => x.UserId == refreshToken.UserId, cancellationToken);
        //     if (result == null || result.Token != refreshToken.Token)
        //         throw new ShiftyException("RefreshToken is not valid");
        //     return true;
        // }
    }
}