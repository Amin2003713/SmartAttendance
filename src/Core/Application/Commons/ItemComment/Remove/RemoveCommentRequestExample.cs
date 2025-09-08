namespace SmartAttendance.Application.Commons.ItemComment.Remove;

public class RemoveCommentRequestExample : IExamplesProvider<RemoveItemCommentRequest>
{
    public RemoveItemCommentRequest GetExamples()
    {
        return new RemoveItemCommentRequest("Hello World", Guid.NewGuid(), Guid.NewGuid());
    }
}