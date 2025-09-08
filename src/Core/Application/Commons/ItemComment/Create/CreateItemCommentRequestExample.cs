namespace SmartAttendance.Application.Commons.ItemComment.Create;

public class CreateItemCommentRequestExample : IExamplesProvider<CreateItemCommentRequest>
{
    public CreateItemCommentRequest GetExamples()
    {
        return new CreateItemCommentRequest("Hello World", Guid.NewGuid());
    }
}