using SmartAttendance.Application.Features.Missions.Requests.Queries.MissionResponse;

namespace SmartAttendance.Application.Features.Missions.Queries.GetMissions;

public record GetMissionsQuery : IRequest<List<GetMissionResponse>>;