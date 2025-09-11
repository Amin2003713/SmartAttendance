using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;

namespace SmartAttendance.Persistence.Db;

public class SmartAttendanceTenantDbContext(
    DbContextOptions<SmartAttendanceTenantDbContext> options
) : EFCoreStoreDbContext<SmartAttendanceTenantInfo>(options)
{
    public DbSet<TenantAdmin> TenantAdmins { get; set; }
    public DbSet<TenantCalendar> TenantCalendars { get; set; }
    public DbSet<TenantUser> TenantUsers { get; set; }

    public DbSet<TenantRequest> TenantRequests { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SmartAttendanceTenantInfo>(builder =>
        {
            // Primary Key
            builder.HasKey(t => t.Id);


            
            // Additional Properties
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            
        });

        // Payments Entity Configuration
        modelBuilder.Entity<TenantAdmin>(builder =>
        {
            // Primary Key
            builder.HasKey(u => u.Id);

            // Relationships
            builder.HasMany(u => u.Tenants)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete behavior

            builder.Property(u => u.RegisteredAt).ValueGeneratedOnAdd();
        });


        modelBuilder.Entity<TenantUser>(builder =>
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.RegisteredAt).ValueGeneratedOnAdd();

            builder.HasOne(a => a.SmartAttendanceTenantInfo).WithMany(t => t.TenantUsers).HasForeignKey(t => t.SmartAttendanceTenantInfoId);

            builder.HasQueryFilter(a => a.IsActive);
        });

        modelBuilder.Entity<TenantCalendar>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.HasQueryFilter(a => a.IsActive);
        });

        modelBuilder.Entity<TenantRequest>(builder => { builder.HasKey(u => u.Id); });
    }
}