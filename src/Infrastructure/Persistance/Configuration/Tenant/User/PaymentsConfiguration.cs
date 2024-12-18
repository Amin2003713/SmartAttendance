using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifty.Domain.Tenants;

namespace Shifty.Persistence.Configuration.Tenant.User;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifty.Domain.Tenants;

public class PaymentsConfiguration : IEntityTypeConfiguration<Payments>
{
    public void Configure(EntityTypeBuilder<Payments> builder)
    {
        // Primary Key
        builder.HasKey(p => p.Id);

        // Configure optional properties
        builder.Property(p => p.CreatedAt).ValueGeneratedOnAdd();

        // // Optional: Configure additional properties
        // builder.Property(p => p.Amount).IsRequired();
        //
        // builder.Property(p => p.Status).HasMaxLength(20);
    }
}