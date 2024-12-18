using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using Shifty.Persistence.Configuration.Tenant.User;
using Shifty.Persistence.Configuration.Users;
using System;

namespace Shifty.Persistence.Db;


public class TenantDbContext(DbContextOptions<TenantDbContext> options) : EFCoreStoreDbContext<ShiftyTenantInfo>(options)
{
    public DbSet<TenantAdmin> Users { get; set; }
    public DbSet<Payments> Payments { get; set; }
    public DbSet<Company> Companies { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations for entities
        modelBuilder.ApplyConfiguration(new ShiftyTenantInfoConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentsConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new TenantAdminConfiguration());

    }
}