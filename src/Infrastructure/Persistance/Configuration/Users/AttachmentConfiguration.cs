using SmartAttendance.Domain.Features.Attachments;

namespace SmartAttendance.Persistence.Configuration.Users;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("Attachments");

        builder.Property(p => p.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.ContentType)
            .HasMaxLength(100);

        builder.HasOne(p => p.Uploader)
            .WithMany()
            .HasForeignKey(p => p.UploadedBy);
    }
}