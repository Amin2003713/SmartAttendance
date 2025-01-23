using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shifty.Common;
using Shifty.Domain.Interfaces.Companies;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using Shifty.Resources.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.Repositories.Companies
{
    public class CompanyRepository : ICompanyRepository , IScopedDependency
    {
        private readonly TenantDbContext            _dbContext;
        private readonly ILogger<CompanyRepository> _logger; // Logger instance
        private readonly CompanyMessages _messages; // Logger instance

        public CompanyRepository(TenantDbContext dbContext , ILogger<CompanyRepository> logger , CompanyMessages messages)
        {
            _dbContext     = dbContext;
            _logger        = logger;
            _messages = messages;
            Entities       = _dbContext.TenantInfo;
        }

        public DbSet<ShiftyTenantInfo> Entities { get; }
        public virtual IQueryable<ShiftyTenantInfo> Table => Entities;
        public virtual IQueryable<ShiftyTenantInfo> TableNoTracking => Entities.AsNoTracking();

        public async Task<bool> IdentifierExistsAsync(string identifier , CancellationToken cancellationToken)
        {
            return await TableNoTracking.AnyAsync(x => x.Identifier == identifier , cancellationToken);
        }


        public async Task<ShiftyTenantInfo> CreateAsync(ShiftyTenantInfo tenantInfo , CancellationToken cancellationToken , bool saveNow = true)
        {
           

            try
            {
                if (await IdentifierExistsAsync(tenantInfo?.Identifier! , cancellationToken))
                    throw ShiftyException.Conflict(additionalData: _messages.Tenant_Exists());

                Entities.Add(tenantInfo);

                if (!saveNow)
                    return tenantInfo;

                await _dbContext.SaveChangesAsync(cancellationToken);
                return await TableNoTracking.SingleOrDefaultAsync(x => x.Identifier == tenantInfo.Identifier , cancellationToken) ?? null!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Source , e);
                throw new ShiftyException(additionalData:"Can't create company Server error");
            }
        }


        public async Task<bool> ValidateDomain(string domain , CancellationToken cancellationToken) =>
            await IdentifierExistsAsync(domain , cancellationToken);


        public async Task<ShiftyTenantInfo> GetByIdAsync(string tenantId , CancellationToken cancellationToken)
        {
            var company = await Table.SingleOrDefaultAsync(a => a.Id == tenantId , cancellationToken);

            if (company == null)
                throw ShiftyException.NotFound(additionalData:"Company not found");

            return company;
        }

        public async Task<IEnumerable<ShiftyTenantInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            var company = await Table.ToListAsync(cancellationToken: cancellationToken);

            if (company.Count == 0)
                throw  ShiftyException.NotFound(additionalData:"Company not found");

            return company;
        }

        public async Task<ShiftyTenantInfo> GetByIdentifierAsync(string identifier , CancellationToken cancellationToken)
        {
            var company = await Table.SingleOrDefaultAsync(a => a.Identifier == identifier , cancellationToken);

            if (company == null)
                throw ShiftyException.NotFound(additionalData: "Company not found");

            return company;
        }
    }
}