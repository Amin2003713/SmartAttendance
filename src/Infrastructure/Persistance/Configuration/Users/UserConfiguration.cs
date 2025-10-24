using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Persistence.Configuration.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(p => p.UserName).IsRequired().HasMaxLength(100);

        builder.HasMany(p => p.SubjectTaught)
            .WithOne(p => p.Teacher)
            .HasForeignKey(p => p.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Users");
    }
}

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