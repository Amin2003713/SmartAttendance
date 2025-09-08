using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;

namespace SmartAttendance.Persistence.Db;

public class SmartAttendanceTenantDbContext(
    DbContextOptions<SmartAttendanceTenantDbContext> options
) : EFCoreStoreDbContext<SmartAttendanceTenantInfo>(options)
{
    public DbSet<TenantAdmin> TenantAdmins { get; set; }
    public DbSet<Payments> Payments { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<TenantDiscount> TenantDiscounts { get; set; }
    public DbSet<TenantCalendar> TenantCalendars { get; set; }
    public DbSet<TenantUser> TenantUsers { get; set; }

    public DbSet<TenantRequest> TenantRequests { get; set; }
    // public DbSet<ActiveService> ActiveServices { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SmartAttendanceTenantInfo>(builder =>
        {
            // Primary Key
            builder.HasKey(t => t.Id);


            builder.HasMany(t => t.Payments)
                .WithOne(p => p.Tenant)
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

            // Additional Properties
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.HasMany(a => a.TenantDiscounts).WithOne(t => t.Tenant).HasForeignKey(t => t.TenantId);
        });

        // Payments Entity Configuration
        modelBuilder.Entity<Payments>(builder =>
        {
            // Primary Key
            builder.HasKey(p => p.Id);

            // Properties
            builder.Property(p => p.CreatedAt).ValueGeneratedOnAdd();
            builder.HasOne(a => a.Discount).WithMany(a => a.Payments).HasForeignKey(a => a.DiscountId);
            builder.HasOne(a => a.Price).WithMany(a => a.Payments).HasForeignKey(a => a.PriceId);

            builder.HasOne(a => a.LastPayment)
                .WithMany()
                .HasForeignKey(a => a.LastPaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(a => a.IsActive);
        });

        // modelBuilder.Entity<ActiveService>(builder =>
        // {
        //     // Primary Key
        //     builder.HasKey(p => p.Id);
        //
        //     builder.HasOne(a => a.Payment).WithMany(a => a.ActiveServices).HasForeignKey(a => a.PaymentId);
        // });

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

            builder.Property(u => u.RegisteredAt).ValueGeneratedOnAdd();
        });


        modelBuilder.Entity<Price>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.CreatedAt).ValueGeneratedOnAdd();
            builder.HasQueryFilter(a => a.IsActive);
        });

        modelBuilder.Entity<Discount>(builder =>
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.CreatedAt).ValueGeneratedOnAdd();

            builder.HasMany(a => a.TenantDiscount).WithOne(t => t.Discount).HasForeignKey(t => t.DiscountId);

            builder.HasQueryFilter(a => a.IsActive);
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