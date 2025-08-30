using Shifty.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackById;

namespace Shifty.Application.Features.TaskTrack.Queries.GetTaskTrackById;

public record GetTaskTrackByIdQuery(
    Guid AggregateId
)
    : IRequest<GetTaskTrackByIdResponse>;