using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;

using Shifty.Persistence.Configuration.Users;
using System;

namespace Shifty.Persistence.Db;


public class TenantDbContext(DbContextOptions<TenantDbContext> options) : EFCoreStoreDbContext<ShiftyTenantInfo>(options)
{
    public DbSet<TenantAdmin> Users { get; set; }
    public DbSet<Payments> Payments { get; set; }


   protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // TenantInfo Entity Configuration
    modelBuilder.Entity<ShiftyTenantInfo>(builder =>
    {
        // Primary Key
        builder.HasKey(t => t.Id);


        builder.HasMany(t => t.Payments)
               .WithOne(p => p.ShiftyTenantInfo)
               .HasForeignKey(p => p.ShiftyTenantInfoId)
               .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

        // Additional Properties
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
    });

    // Payments Entity Configuration
    modelBuilder.Entity<Payments>(builder =>
    {
        // Primary Key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.CreatedAt).ValueGeneratedOnAdd();
    });


    // TenantAdmin Entity Configuration
    modelBuilder.Entity<TenantAdmin>(builder =>
    {
        // Primary Key
        builder.HasKey(u => u.Id);

        // Relationships
        builder.HasMany(u => u.Tenants)
               .WithOne(t => t.User)
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Restrict); // Restrict delete behavior
    });
}

}