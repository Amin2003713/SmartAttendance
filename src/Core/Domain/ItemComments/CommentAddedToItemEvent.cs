namespace Shifty.Domain.ItemComments;

public record CommentAddedToItemEvent(
    string Text,
    Guid UserId
) : DomainEvent;