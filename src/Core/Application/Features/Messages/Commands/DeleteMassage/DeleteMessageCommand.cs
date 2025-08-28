namespace Shifty.Application.Features.Messages.Commands.DeleteMassage;

public record DeleteMessageCommand(
    Guid MessageId
) : IRequest;