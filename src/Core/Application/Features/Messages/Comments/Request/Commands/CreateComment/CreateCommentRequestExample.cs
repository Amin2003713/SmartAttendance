namespace Shifty.Application.Features.Messages.Comments.Request.Commands.CreateComment;

public class CreateCommentRequestExample : IExamplesProvider<CreateCommentRequest>
{
    public CreateCommentRequest GetExamples()
    {
        return new CreateCommentRequest
        {
            Text = "کامنت",
            MessageId = Guid.Empty,
            RelatedCommentId = Guid.Empty
        };
    }
}