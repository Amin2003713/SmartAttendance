namespace Shifty.Application.Features.Messages.Comments.Request.Commands.UpdateComment;

public class UpdateCommentRequest
{
    public Guid Id { get; set; }
    public string? Text { get; set; }
    public Guid MessageId { get; set; }
    public Guid? RelatedCommentId { get; set; }
}