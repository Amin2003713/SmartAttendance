using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.Excuses;

namespace SmartAttendance.Persistence.Configuration.Users;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("Attendance");

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(p => p.Enrollment)
            .WithOne(p => p.Attendance)
            .HasForeignKey<Attendance>(a => a.EnrollmentId);

        builder.HasOne(p => p.Excuse)
            .WithOne(u => u.Attendance)
            .HasForeignKey<Attendance>(a => a.ExcuseId);
    }
}