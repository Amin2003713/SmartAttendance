namespace Shifty.Application.Features.Messages.Commands.LikeMessage;

public record LikeMessageCommand(
    Guid MessageId
) : IRequest;