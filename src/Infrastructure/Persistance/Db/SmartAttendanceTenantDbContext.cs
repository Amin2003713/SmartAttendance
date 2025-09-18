using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;

namespace SmartAttendance.Persistence.Db;

public class SmartAttendanceTenantDbContext(
    DbContextOptions<SmartAttendanceTenantDbContext> options
) : EFCoreStoreDbContext<UniversityTenantInfo>(options)
{
    public DbSet<UniversityAdmin> UniversityAdmins { get; set; }
    public DbSet<TenantCalendar> TenantCalendars { get; set; }
    public DbSet<UniversityUser> UniversityUsers { get; set; }
    public DbSet<TenantRequest> TenantRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // UniversityTenantInfo
        modelBuilder.Entity<UniversityTenantInfo>(builder =>
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(t => t.BranchName)
                .HasMaxLength(100);

            builder.HasMany(t => t.Users)
                .WithOne(u => u.UniversityTenantInfo)
                .HasForeignKey(u => u.UniversityTenantInfoId);
        });

        // UniversityAdmin
        modelBuilder.Entity<UniversityAdmin>(builder =>
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Tenants)
                .WithOne(t => t.BranchAdmin)
                .HasForeignKey(t => t.BranchAdminId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(u => u.RegisteredAt)
                .ValueGeneratedOnAdd();
        });

        // UniversityUser
        modelBuilder.Entity<UniversityUser>(builder =>
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.RegisteredAt)
                .ValueGeneratedOnAdd();

            builder.HasOne(u => u.UniversityTenantInfo)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.UniversityTenantInfoId);

            builder.HasQueryFilter(u => u.IsActive);
        });

        // TenantCalendar
        modelBuilder.Entity<TenantCalendar>(builder =>
        {
            builder.HasKey(c => c.Id);
            builder.HasQueryFilter(c => c.IsActive);
        });

        // TenantRequest
        modelBuilder.Entity<TenantRequest>(builder =>
        {
            builder.HasKey(r => r.Id);
        });
    }
}