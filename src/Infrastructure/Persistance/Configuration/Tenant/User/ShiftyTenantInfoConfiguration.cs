using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifty.Domain.Tenants;

namespace Shifty.Persistence.Configuration.Tenant.User
{
    public class ShiftyTenantInfoConfiguration : IEntityTypeConfiguration<ShiftyTenantInfo>
    {
        public void Configure(EntityTypeBuilder<ShiftyTenantInfo> builder)
        {
            // Primary Key
            builder.HasKey(t => t.Id);

            // Relationships
            builder.HasOne(t => t.User).
                    WithMany(u => u.Tenants) // Assuming a User can have many tenants
                    .
                    HasForeignKey(t => t.UserId).
                    OnDelete(DeleteBehavior.Restrict); // Optional: define how to handle delete (Restrict, Cascade, etc.)

            builder.HasOne(t => t.Company).
                    WithOne(c => c.TenantInfos).
                    HasForeignKey<Company>(c => c.TenantInfosId) // Explicitly define the FK
                    .
                    OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Payments).
                    WithOne(p => p.ShiftyTenantInfo) // Duplicate navigation reference
                    .
                    HasForeignKey(p => p.ShiftyTenantInfoId) // Correct property reference
                    .
                    OnDelete(DeleteBehavior.Cascade);

            // Optional: Configure additional properties
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        }
    }
}