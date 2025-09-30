using SmartAttendance.Domain.Features.Notifications;

namespace SmartAttendance.Persistence.Configuration.Users;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(p => p.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(p => p.UserId);
    }
}