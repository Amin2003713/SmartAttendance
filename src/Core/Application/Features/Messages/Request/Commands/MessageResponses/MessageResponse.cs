namespace Shifty.Application.Features.Messages.Request.Commands.MessageResponses;

public class MessageResponse
{
    public Guid? Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public int Visit { get; set; }
    public int Like { get; set; }
    public string UserId { get; set; }
    public DateTime Reported { get; set; }
    public int? CommentCount { get; set; }
    public string InserterName { get; set; } = null!;
}