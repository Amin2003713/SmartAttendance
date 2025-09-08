namespace SmartAttendance.Application.Commons.ItemComment.Remove;

public class RemoveItemCommentRequest(
    string Text,
    Guid UserId,
    Guid AggregateId
);