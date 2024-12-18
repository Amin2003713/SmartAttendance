using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using System;

namespace Shifty.Persistence.Db;


public class TenantDbContext(DbContextOptions<TenantDbContext> options) : EFCoreStoreDbContext<ShiftyTenantInfo>(options)
{
    public DbSet<TenantAdmin> Users { get; set; }
    public DbSet<Payments> Payments { get; set; }
    public DbSet<Company> Companies { get; set; }
}