using Shifty.Application.Features.Missions.Requests.Queries.MissionResponse;

namespace Shifty.Application.Features.Missions.Queries.GetMissions;

public record GetMissionsQuery : IRequest<List<GetMissionResponse>>;