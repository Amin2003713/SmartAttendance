using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Persistence.Configuration.Users;

public class MajorConfiguration : IEntityTypeConfiguration<Major>
{
    public void Configure(EntityTypeBuilder<Major> builder)
    {
        builder.HasMany(p => p.Subjects)
            .WithOne(u => u.Major)
            .HasForeignKey(p => p.MajorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}