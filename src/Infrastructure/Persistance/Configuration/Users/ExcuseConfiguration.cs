using SmartAttendance.Domain.Features.Excuses;

namespace SmartAttendance.Persistence.Configuration.Users;

public class ExcuseConfiguration : IEntityTypeConfiguration<Excuse>
{
    public void Configure(EntityTypeBuilder<Excuse> builder)
    {
        builder.ToTable("Excuses");

        builder.Property(p => p.Reason)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(p => p.Student)
            .WithMany(u => u.Excuses)
            .HasForeignKey(p => p.StudentId);

        builder.HasOne(p => p.Plan)
            .WithMany()
            .HasForeignKey(p => p.PlanId);

        builder.HasOne(p => p.Attachment)
            .WithMany()
            .HasForeignKey(p => p.AttachmentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}