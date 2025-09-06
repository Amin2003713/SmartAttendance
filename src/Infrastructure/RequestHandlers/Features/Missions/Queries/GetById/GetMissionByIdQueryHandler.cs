using Mapster;
using Shifty.Application.Features.Missions.Queries.GetById;
using Shifty.Application.Features.Missions.Requests.Queries.MissionResponse;
using Shifty.Application.Features.Users.Queries.GetAllUsers;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using Shifty.Common.Exceptions;
using Shifty.Domain.Missions.Aggregate;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.Missions.Queries.GetById;

public class GetMissionByIdQueryHandler(
    IdentityService identityService,
    ILogger<GetMissionByIdQueryHandler> logger,
    IMediator mediator,
    IEventReader<Mission, Guid> eventReader) : IRequestHandler<GetMissionByIdQuery, GetMissionResponse>
{
    public async Task<GetMissionResponse> Handle(GetMissionByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = identityService.GetUserId<Guid>();
        logger.LogInformation("User {UserId} requested Missions with ID {AggregateId} ",
            userId,
            request.AggregateId);


        var events = await eventReader.LoadEventsAsync(request.AggregateId, cancellationToken);

        if (events is null || events.Count == 0)
        {
            logger.LogWarning("No events found for Missions {AggregateId}", request.AggregateId);
            throw ShiftyException.NotFound("Task not found");
        }

        var mission = new Mission();

        var users = await mediator.Send(new GetAllUsersQuery(), cancellationToken);
        var userDictionary = users.ToDictionary(u => u.Id);

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


        mission.LoadFromHistory(events);


        var response = mission.Adapt<GetMissionResponse>();

        logger.LogInformation(
            "Successfully retrieved Missions {AggregateId} for user {UserId}",
            request.AggregateId,
            userId);

        return response;
    }
}