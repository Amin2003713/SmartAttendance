namespace Shifty.Persistence.Configuration.Users;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoles>
{
    public void Configure(EntityTypeBuilder<UserRoles> builder)
    {
        builder.HasKey(a => a.Id);

        builder.ToTable("UserRoles");
    }
}