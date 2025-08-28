using Shifty.Application.Features.Messages.Comments.Request.Queries.GetComments;
using Shifty.Application.Features.Messages.Request.Commands.CreateMessage;

namespace Shifty.Application.Features.Messages.Request.Queries.GetMessageById;

public class GetMessageByIdResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string User { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageCompressed { get; set; }

    public int Count { get; set; }
    public bool IsLiked { get; set; }
    public int Like { get; set; }
    public int CommentCount { get; set; }
    public bool Seen { get; set; }
    public Guid? UserId { get; set; }
    public List<UserMessageResponse> Recipients { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Visit { get; set; }
    public List<GetCommentResponse> Comments { get; set; }
}