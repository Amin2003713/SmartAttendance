using Mapster;
using Shifty.Application.Features.Missions.Queries.GetMissions;
using Shifty.Application.Features.Missions.Requests.Queries.MissionResponse;
using Shifty.Application.Features.Users.Queries.GetAllUsers;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using Shifty.Common.Exceptions;
using Shifty.Domain.Missions.Aggregate;

namespace Shifty.RequestHandlers.Features.Missions.Queries.GetMission;

public class GetMissionsQueryHandler(
    ILogger<GetMissionsQueryHandler> logger,
    IStringLocalizer<GetMissionsQueryHandler> localizer,
    IMediator mediator,
    IEventReader<Mission, Guid> eventReader) : IRequestHandler<GetMissionsQuery, List<GetMissionResponse>>
{
    public async Task<List<GetMissionResponse>> Handle(GetMissionsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var events = await eventReader.LoadHybridAsync(null!, null!, cancellationToken);
            List<GetMissionResponse> missions = [];

            var users = await mediator.Send(new GetAllUsersQuery(), cancellationToken);
            var userDictionary = users.ToDictionary(u => u.Id);

            foreach (var mission in events)
            {
                var missionResponse = mission.Adapt<GetMissionResponse>();

                if (userDictionary.TryGetValue(mission.EmployeeId, out var user))
                {
                    missionResponse.Employee = user.Adapt<LogPropertyInfoResponse>();
                }
                else
                {
                    logger.LogWarning("User with ID {EmployeeId} not found.", mission.EmployeeId);
                    missionResponse.Employee = null;
                }

                missions.Add(missionResponse);
            }

            logger.LogDebug("Loaded {Count} mission records.", missions.Count);
            return missions;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading mission events");
            throw ShiftyException.InternalServerError(
                localizer["An error occurred while retrieving mission data."].Value);
        }
    }
}