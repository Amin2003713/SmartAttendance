namespace Shifty.Domain.ItemComments;

public record CommentDeletedFromItemEvent(
    Guid UserId,
    string Text
) : DomainEvent;