namespace Shifty.Domain.ItemComments;

public record AggregateComment(
    DateTime AddedTime,
    string Text,
    Guid UserId
);