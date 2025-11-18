using SmartAttendance.Domain.Features.PlanEnrollments;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Persistence.Configuration.Users;

public class PlanEnrollmentConfiguration : IEntityTypeConfiguration<PlanEnrollment>
{
    public void Configure(EntityTypeBuilder<PlanEnrollment> builder)
    {
        builder.ToTable("PlanEnrollments");

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(p => p.Plan)
            .WithMany(p => p.Enrollments)
            .HasForeignKey(p => p.PlanId);

        builder.HasOne(p => p.Student)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(p => p.StudentId);

        builder.HasQueryFilter(a => a.IsActive);

    }
}