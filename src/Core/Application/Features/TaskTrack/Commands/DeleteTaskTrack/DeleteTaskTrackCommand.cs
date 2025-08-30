namespace Shifty.Application.Features.TaskTrack.Commands.DeleteTaskTrack;

public record DeleteTaskTrackCommand(
    Guid AggregateId
) : IRequest;