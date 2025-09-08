namespace SmartAttendance.Domain.ItemComments;

public record CommentDeletedFromItemEvent(
    Guid UserId,
    string Text
) : DomainEvent;