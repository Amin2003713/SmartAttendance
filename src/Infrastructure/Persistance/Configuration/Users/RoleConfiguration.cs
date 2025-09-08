using Role = SmartAttendance.Domain.Users.Role;

namespace SmartAttendance.Persistence.Configuration.Users;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(50);

        builder.ToTable("RoleTypes");
    }
}