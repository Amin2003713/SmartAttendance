namespace SmartAttendance.Domain.ItemComments;

public record CommentAddedToItemEvent(
    string Text,
    Guid UserId
) : DomainEvent;