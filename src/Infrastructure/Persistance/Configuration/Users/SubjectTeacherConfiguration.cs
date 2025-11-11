using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Configuration.Users;

public class SubjectTeacherConfiguration : IEntityTypeConfiguration<SubjectTeacher>
{
    public void Configure(EntityTypeBuilder<SubjectTeacher> builder)
    {
        builder.HasOne(st => st.Subject)
            .WithMany(s => s.Teachers)
            .HasForeignKey(st => st.SubjectId)
            .IsRequired();

        builder.HasOne(st => st.Teacher)
            .WithMany(u => u.SubjectTaught)
            .HasForeignKey(st => st.TeacherId)
            .IsRequired();
    }
}