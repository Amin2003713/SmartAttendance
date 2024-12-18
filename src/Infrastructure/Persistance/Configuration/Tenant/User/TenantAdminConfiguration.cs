using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifty.Domain.Tenants;

namespace Shifty.Persistence.Configuration.Tenant.User
{
    public class TenantAdminConfiguration : IEntityTypeConfiguration<TenantAdmin>
    {
        public void Configure(EntityTypeBuilder<TenantAdmin> builder)
        {
            // Primary Key
            builder.HasKey(u => u.Id);

            // Configure the navigation property for tenants
            builder.HasMany(u => u.Tenants).WithOne(t => t.User).HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Restrict);

            // Optional: Configure additional properties
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(100);
        }
    }
}