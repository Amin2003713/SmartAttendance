namespace Shifty.Application.Features.Messages.Comments.Request.Commands.CreateComment;

public class CreateCommentRequest
{
    public string? Text { get; set; }
    public Guid MessageId { get; set; }
    public Guid? RelatedCommentId { get; set; }
}