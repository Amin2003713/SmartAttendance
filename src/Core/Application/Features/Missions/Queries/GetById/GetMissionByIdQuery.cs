using SmartAttendance.Application.Features.Missions.Requests.Queries.MissionResponse;

namespace SmartAttendance.Application.Features.Missions.Queries.GetById;

public record GetMissionByIdQuery(
    Guid AggregateId
) : IRequest<GetMissionResponse>;