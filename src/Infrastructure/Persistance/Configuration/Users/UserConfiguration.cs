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