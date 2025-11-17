using SmartAttendance.Domain.Features.Plans;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Configuration.Users;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.ToTable("Plans");

        builder.Property(p => p.CourseName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder
            .OwnsOne(p => p.Location,
                l =>
                {
                    l.Property(x => x.Lat).HasColumnName("Location_Lat");
                    l.Property(x => x.Lng).HasColumnName("Location_Lng");
                    l.Property(x => x.Name).HasColumnName("Location_Name");
                });

        builder.Property(p => p.Capacity)
            .IsRequired();

        builder.HasMany(p => p.Subjects)
            .WithOne(u => u.Plan)
            .HasForeignKey(p => p.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(a => a.IsActive);
    }
}