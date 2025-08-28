namespace Shifty.Application.Features.Messages.Commands.VisitMessage;

public record VisitMessageCommand(
    Guid Id
) : IRequest;