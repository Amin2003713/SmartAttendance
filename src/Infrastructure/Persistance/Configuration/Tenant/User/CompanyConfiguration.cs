using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifty.Domain.Tenants;

namespace Shifty.Persistence.Configuration.Tenant.User
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            // Primary Key
            builder.HasKey(c => c.Id);

            // Configure relationships
            builder.HasOne(c => c.TenantInfos).WithOne(t => t.Company).HasForeignKey<Company>(c => c.TenantInfosId).OnDelete(DeleteBehavior.Restrict);

            // Optional: Configure additional properties
            builder.Property(c => c.Name).IsRequired().HasMaxLength(150);
        }
    }
}