using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shifty.Application.Users.Exceptions; // Added for logging
using Shifty.Common;
using Shifty.Common.Utilities;
using Shifty.Domain.Interfaces.Users;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.Repositories.Users
{
    public class UserRepository(WriteOnlyDbContext dbContext , ILogger<UserRepository> logger , ILogger<Repository<User , WriteOnlyDbContext>> repoLogger)
        : Repository<User , WriteOnlyDbContext>(dbContext , repoLogger) , IUserRepository , IScopedDependency
    {
        // Constructor with dependency injection for DbContext and Loggers

        /// <summary>
        /// Updates the last login date of the specified user.
        /// </summary>
        /// <param name="user">The user whose last login date is to be updated.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                user.LastLoginDate = DateTime.UtcNow;
                await UpdateAsync(user, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating last login date for user: {UserId}", user.Id);
                throw  ShiftyException.InternalServerError();
            }
        }

        /// <summary>
        /// Retrieves a user by username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The plaintext password of the user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The matching user if found; otherwise, null.</returns>
        public async Task<User> GetByUserAndPassAsync(string username, string password, CancellationToken cancellationToken)
        {
            try
            {
                var passwordHash = SecurityHelper.GetSha256Hash(password);
                var user = await Table
                    .Where(p => p.UserName == username && p.PasswordHash == passwordHash)
                    .SingleOrDefaultAsync(cancellationToken);

                if (user == null)
                    return user;

                logger.LogInformation("User found with username: {Username}", username);
                throw ShiftyException.NotFound(additionalData: UserExceptions.User_NotFound);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving user with username: {Username}", username);
                throw ShiftyException.InternalServerError();

            }
        }

        /// <summary>
        /// Updates the security stamp of the specified user.
        /// </summary>
        /// <param name="user">The user whose security stamp is to be updated.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating security stamp for user: {UserId}", user.Id);
            try
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
                await UpdateAsync(user, cancellationToken);
                logger.LogInformation("Successfully updated security stamp for user: {UserId}", user.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating security stamp for user: {UserId}", user.Id);
                throw ShiftyException.InternalServerError();
            }
        }

        /// <summary>
        /// Adds a new user with the specified password.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <param name="password">The plaintext password for the user.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddAsync(User user, string password, CancellationToken cancellationToken)
        {
            logger.LogInformation("Attempting to add new user with username: {Username}", user.UserName);
            try
            {
                var exists = await TableNoTracking.AnyAsync(p => p.UserName == user.UserName, cancellationToken);
                if (exists)
                {
                    logger.LogWarning("Cannot add user. Username already exists: {Username}", user.UserName);
                    throw ShiftyException.Conflict(additionalData:UserExceptions.User_Already_Exists);
                }

                var passwordHash = SecurityHelper.GetSha256Hash(password);
                user.PasswordHash = passwordHash;

                await base.AddAsync(user, cancellationToken);
                logger.LogInformation("Successfully added new user with username: {Username}", user.UserName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding new user with username: {Username}", user.UserName);
                throw ShiftyException.InternalServerError();
            }
        }
    }
}
