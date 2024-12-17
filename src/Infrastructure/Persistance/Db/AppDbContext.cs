using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Users;
using Shifty.Persistence.TenantServices;
using System;

namespace Shifty.Persistence.Db;

using Common.Utilities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public class AppDbContext : IdentityDbContext<User, Role, Guid>, IAppDbContext
{
    private readonly ITenantService _tenantService;

    public AppDbContext()
    {
        
    }
    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService tenantService) : base(options)
    {
        _tenantService = tenantService;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _tenantService.GetConnectionString();
            optionsBuilder.UseSqlServer(connectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entitiesAssembly = typeof(IEntity).Assembly;

        modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IEntity).Assembly);
        modelBuilder.AddPluralizingTableNameConvention();
    }

    public async Task<int> ExecuteSqlRawAsync(string query, CancellationToken cancellationToken)
    {
        var result = await base.Database.ExecuteSqlRawAsync(query, cancellationToken);
        return result;
    }

    public async Task<int> ExecuteSqlRawAsync(string query) => await ExecuteSqlRawAsync(query, CancellationToken.None);
}