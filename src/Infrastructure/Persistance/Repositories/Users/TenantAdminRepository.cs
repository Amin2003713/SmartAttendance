using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shifty.Common;
using Shifty.Common.Utilities;
using Shifty.Domain.Interfaces.Base;
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
    public class TenantAdminRepository(TenantDbContext dbContext) : ITenantAdminRepository, IScopedDependency
    {
        private DbSet<TenantAdmin> Entities { get; } = dbContext.Users;
        protected virtual IQueryable<TenantAdmin> Table => Entities;
        protected virtual IQueryable<TenantAdmin> TableNoTracking => Entities.AsNoTracking();

        public async Task<bool> UserExists(string phoneNumber , CancellationToken cancellationToken) =>
            await TableNoTracking.AnyAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);



        public async Task<TenantAdmin> CreateAsync(TenantAdmin user , string companyId , CancellationToken cancellationToken)
        {
            try
            {
                var userResult = await GetByCompanyAsync(companyId , cancellationToken);
                if (userResult != null)
                    return userResult;

                await Entities.AddAsync(user , cancellationToken);
                 await dbContext.SaveChangesAsync(cancellationToken);
                 return user;
            }
            catch (Exception e)
            {
                throw new ShiftyException(ApiResultStatusCode.ServerError , $"You are unauthorized to access this resource. {e}" , HttpStatusCode.InternalServerError);
            }
        }

        public async Task<TenantAdmin> GetByCompanyAsync(string companyId , CancellationToken cancellationToken) =>
            (await TableNoTracking.SingleOrDefaultAsync(a => a.Tenants.Exists(w => w.Id == companyId) , cancellationToken)) ??
            throw new ShiftyException(ApiResultStatusCode.NotFound , "User does not exist.");
    }
}