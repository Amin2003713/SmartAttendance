namespace Shifty.Application.Commons.ItemComment.Create;

public record CreateItemCommentRequest(
    string Text,
    Guid AggregateId
);