using Microsoft.EntityFrameworkCore;
using Shifty.Common;
using Shifty.Domain.Interfaces.Users;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.Repositories.Users
{
    public class TenantAdminRepository(TenantDbContext dbContext) : ITenantAdminRepository , IScopedDependency
    {
        private DbSet<TenantAdmin> Entities { get; } = dbContext.Users;
        protected virtual IQueryable<TenantAdmin> Table => Entities;
        protected virtual IQueryable<TenantAdmin> TableNoTracking => Entities.AsNoTracking();

        public async Task<bool> UserExists(string phoneNumber , CancellationToken cancellationToken)
        {
            return await TableNoTracking.AnyAsync(u => u.PhoneNumber == phoneNumber , cancellationToken);
        }


        public async Task<TenantAdmin> CreateAsync(TenantAdmin user , CancellationToken cancellationToken)
        {
            try
            {
                var userResult = await GetByCompanyOrPhoneNumber(user.PhoneNumber , cancellationToken);
                if (userResult != null)
                    return userResult;

                Entities.Add(user);
                await dbContext.SaveChangesAsync(cancellationToken);
                return user;
            }
            catch (Exception e)
            {
                throw new ShiftyException(ApiResultStatusCode.ServerError
                    , $"You are unauthorized to access this resource. {e}"
                    , HttpStatusCode.InternalServerError);
            }
        }

        public async Task<TenantAdmin> GetByCompanyOrPhoneNumberAsync(string phoneNumber , CancellationToken cancellationToken)
        {
            return (await GetByCompanyOrPhoneNumber(phoneNumber , cancellationToken)) ??
                   throw new ShiftyException(ApiResultStatusCode.NotFound , "User does not exist.");
        }


        private async Task<TenantAdmin> GetByCompanyOrPhoneNumber(string phoneNumber , CancellationToken cancellationToken)
        {
            return (await Table.SingleOrDefaultAsync(a => a.PhoneNumber == phoneNumber , cancellationToken)) ??
                   null!;
        }
    }
}