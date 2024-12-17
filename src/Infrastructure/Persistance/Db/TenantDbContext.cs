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



protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Configuring ShiftyTenantInfo
    modelBuilder.Entity<ShiftyTenantInfo>(entity =>
    {
        // Primary Key
        entity.HasKey(t => t.Id);

        // Relationships
        entity.HasOne(t => t.User)
              .WithMany(t=>t.Tenants) // Assuming the user can be associated with multiple tenants
              .HasForeignKey(t => t.UserId)
              .OnDelete(DeleteBehavior.Restrict); // Optional: define how to handle delete (Restrict, Cascade, etc.)

        entity.HasOne(t => t.Company).
               WithOne(c => c.TenantInfos).
               HasForeignKey<Company>(c => c.TenantInfosId) // Explicitly define the FK
               .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(t => t.Payments).
               WithOne(a => a.ShiftyTenantInfo) // Duplicate navigation reference
               .HasForeignKey(p => p.ShiftyTenantInfoId) // Wrong property reference
               .OnDelete(DeleteBehavior.Cascade);
    });

    // Configuring Payments
    modelBuilder.Entity<Payments>(entity =>
    {
        // Primary Key
        entity.HasKey(p => p.Id);

        // Optional: define properties
        entity.Property(p => p.CreatedAt)
              .ValueGeneratedOnAdd();
    });


    // Other entity configurations go here...
}

}