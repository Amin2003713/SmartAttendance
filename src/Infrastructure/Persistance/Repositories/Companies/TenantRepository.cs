using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shifty.Application.Common;
using Shifty.Common;
using Shifty.Domain.Interfaces.Companies;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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


        public async Task<ShiftyTenantInfo> CreateAsync(ShiftyTenantInfo tenantInfo , CancellationToken cancellationToken , bool saveNow = true)
        {
            if(await IdentifierExistsAsync(tenantInfo?.Identifier! , cancellationToken))
                throw new ShiftyException(ApiResultStatusCode.BadRequest , "Tenant cannot be created");

            try
            {
                Entities.Add(tenantInfo);

                if (!saveNow)
                    return tenantInfo;

                await DbContext.SaveChangesAsync(cancellationToken);
                return await TableNoTracking.SingleOrDefaultAsync(x => x.Identifier == tenantInfo.Identifier, cancellationToken) ?? null!;
            }
            catch (Exception e)
            {
                throw new ShiftyException(ApiResultStatusCode.DataBaseError , "Can't create company Server error");
            }
        }

        public async Task<bool> ExistsAsync(string identifierId , CancellationToken cancellationToken) =>
            await TableNoTracking
                .AnyAsync(x => x.Identifier == identifierId ||
                               x.Id         == identifierId,
                    cancellationToken);

        public async Task<(bool IsValid , string message)> ValidateDomain(string domain , CancellationToken cancellationToken)
        {
            // Check if the domain is empty or null
            if (string.IsNullOrEmpty(domain))
                return (false , "Domain cannot be empty or null.");

            if (domain.Length > 63)
                return (false , "");

            // Check if the domain starts or ends with a hyphen
            if (domain.StartsWith("-") || domain.EndsWith("-"))
                return (false , ResponseMessageConstant.Company.CheckDomainQuery.Failed);

            // Check if the domain contains only allowed characters (a-z, A-Z, 0-9, -)
            if (!Regex.IsMatch(domain , @"^[a-zA-Z0-9-]+$"))
            {
                var invalidChars = Regex.Replace(domain , @"[a-zA-Z0-9-]" , "");
                return (false , ResponseMessageConstant.Company.CheckDomainQuery.Failed);
            }

            if (!await IdentifierExistsAsync(domain, cancellationToken))
                return (true , ResponseMessageConstant.Company.CheckDomainQuery.Success);

            return (false , ResponseMessageConstant.Company.CheckDomainQuery.Failed);
        }


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
    }
}