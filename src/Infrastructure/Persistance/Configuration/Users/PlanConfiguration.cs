using SmartAttendance.Domain.Features.Plans;

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

        builder.Property(p => p.Location)
            .HasMaxLength(200);

        builder.Property(p => p.Capacity)
            .IsRequired();

        builder.HasMany(p => p.Subjects)
            .WithOne(u => u.Plan)
            .HasForeignKey(p => p.PlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}