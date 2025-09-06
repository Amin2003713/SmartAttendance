using Shifty.Application.Features.Missions.Requests.Queries.MissionResponse;

namespace Shifty.Application.Features.Missions.Queries.GetById;

public record GetMissionByIdQuery(Guid AggregateId) : IRequest<GetMissionResponse>;