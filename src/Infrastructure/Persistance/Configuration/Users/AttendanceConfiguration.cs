using SmartAttendance.Domain.Features.Attendances;

namespace SmartAttendance.Persistence.Configuration.Users;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("Attendance");

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(p => p.Plan)
            .WithMany(p => p.Attendances)
            .HasForeignKey(p => p.PlanId);

        builder.HasOne(p => p.Student)
            .WithMany(u => u.Attendances)
            .HasForeignKey(p => p.StudentId);

        builder.HasOne(p => p.Excuse)
            .WithMany()
            .HasForeignKey(p => p.ExcuseId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}