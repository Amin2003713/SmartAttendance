using Shifty.Common.General.BaseClasses;

namespace Shifty.Domain.Messages.Comments;

public class Comment : BaseEntity
{
    public string? Text { get; set; }
    public Guid MessageId { get; set; }
    public Message Message { get; set; }

    public Comment? RelatedComment { get; set; }
    public Guid? RelatedCommentId { get; set; }
}