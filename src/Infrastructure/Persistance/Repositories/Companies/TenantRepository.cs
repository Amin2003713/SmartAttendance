using Microsoft.EntityFrameworkCore;
using Shifty.Common;
using Shifty.Domain.Interfaces.Companies;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.Repositories.Tenants
{
    public class CompanyRepository : ICompanyRepository , IScopedDependency
    {

        protected readonly TenantDbContext DbContext;
        public DbSet<ShiftyTenantInfo> Entities { get; }
        public virtual IQueryable<ShiftyTenantInfo> Table => Entities;
        public virtual IQueryable<ShiftyTenantInfo> TableNoTracking => Entities.AsNoTracking();

        public CompanyRepository(TenantDbContext dbContext)
        {
            DbContext = dbContext;
            Entities = DbContext.TenantInfo;
        }

        public async Task<bool> IdentifierExistsAsync(string identifier , CancellationToken cancellationToken)
        {
            return await TableNoTracking.AnyAsync(x => x.Identifier == identifier, cancellationToken: cancellationToken);
        }

        public async Task<bool> NationalIdExistsAsync(string nationalId, CancellationToken cancellationToken)
        {
            return await TableNoTracking.AnyAsync(x => x.NationalId == nationalId, cancellationToken: cancellationToken);
        }

        public async Task<bool> RegistrationNumberExistsAsync(string registrationNumber, CancellationToken cancellationToken)
        {
            return await TableNoTracking.AnyAsync(x => x.RegistrationNumber == registrationNumber, cancellationToken: cancellationToken);
        }

        public async Task<bool> CreateAsync(ShiftyTenantInfo tenantInfo, CancellationToken cancellationToken)
        {
            if(!await CanCreateAsync(tenantInfo , cancellationToken))
                throw new ShiftyException(ApiResultStatusCode.BadRequest , "Tenant cannot be created");

            try
            {
                Entities.Add(tenantInfo);
                await DbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> ExistsAsync(string identifierId , CancellationToken cancellationToken) =>
            await TableNoTracking
                .AnyAsync(x => x.Identifier == identifierId ||
                               x.Id         == identifierId,
                    cancellationToken);


        public async Task<ShiftyTenantInfo> GetByIdAsync(string tenantId, CancellationToken cancellationToken)
        {
           var company =await Table.SingleOrDefaultAsync(a=>a.Id == tenantId, cancellationToken: cancellationToken);

            if(company == null)
                throw new ShiftyException(ApiResultStatusCode.NotFound,"Company not found");

            return company;
        }

        public async Task<IEnumerable<ShiftyTenantInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            var company = await Table.ToListAsync(cancellationToken: cancellationToken);

            if (company.Count == 0)
                throw new ShiftyException(ApiResultStatusCode.NotFound, "Company not found");

            return company;
        }

        public async Task<ShiftyTenantInfo> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken)
        {
            var company = await Table.SingleOrDefaultAsync(a => a.Identifier == identifier, cancellationToken: cancellationToken);

            if (company == null)
                throw new ShiftyException(ApiResultStatusCode.NotFound, "Company not found");

            return company;
        }

        public async Task<bool> CanCreateAsync(ShiftyTenantInfo tenantInfo, CancellationToken cancellationToken)
        {
            return !(
                    await TableNoTracking.AnyAsync(a=>a.PostalCode         == tenantInfo.PostalCode,         cancellationToken: cancellationToken) ||
                    await TableNoTracking.AnyAsync(a=>a.RegistrationNumber == tenantInfo.RegistrationNumber, cancellationToken: cancellationToken) ||
                    await TableNoTracking.AnyAsync(a=>a.Identifier         == tenantInfo.Identifier,         cancellationToken: cancellationToken) 
                    );
        }
    }
}