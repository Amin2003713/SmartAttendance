namespace Shifty.Application.Features.Messages.Comments.Request.Queries.GetComments;

public class GetCommentResponse
{
    public Guid Id { get; set; }
    public string? Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Author { get; set; }

    public Guid? ParentId { get; set; }
}