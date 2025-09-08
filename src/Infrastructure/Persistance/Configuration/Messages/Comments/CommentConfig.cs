using SmartAttendance.Domain.Messages.Comments;

namespace SmartAttendance.Persistence.Configuration.Messages.Comments;

public class CommentConfig : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(c => c.MessageId).IsRequired();

        builder.HasOne(c => c.Message).WithMany(m => m.Comments).HasForeignKey(c => c.MessageId);

        builder.HasOne(c => c.RelatedComment).WithMany().HasForeignKey(c => c.RelatedCommentId).IsRequired(false);
    }
}