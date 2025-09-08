using SmartAttendance.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackById;

namespace SmartAttendance.Application.Features.TaskTrack.Queries.GetTaskTrackById;

public record GetTaskTrackByIdQuery(
    Guid AggregateId
)
    : IRequest<GetTaskTrackByIdResponse>;