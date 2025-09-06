using Shifty.Application.Features.Missions.Commands.Create;
using Shifty.Application.Features.Missions.Commands.Delete;
using Shifty.Application.Features.Missions.Commands.Update;
using Shifty.Application.Features.Missions.Queries.GetById;
using Shifty.Application.Features.Missions.Queries.GetMissions;
using Shifty.Application.Features.Missions.Requests.Commands.Create;
using Shifty.Application.Features.Missions.Requests.Commands.Update;
using Shifty.Application.Features.Missions.Requests.Queries.MissionResponse;

namespace Shifty.Api.Controllers.Missions;

public class MissionController : ShiftyBaseController
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create Missions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task Create([FromBody] CreateMissionRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateMissionCommand>(), cancellationToken);
    }


    [HttpPut]
    [SwaggerOperation(Summary = "Put Missions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task Update([FromBody] UpdateMissionRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateMissionCommand>(), cancellationToken);
    }

    [HttpDelete]
    [SwaggerOperation(Summary = "Delete Missions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task Delete(Guid aggregateId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteMissionCommand(aggregateId), cancellationToken);
    }

    [HttpGet]
    [SwaggerOperation("Get Mission for a project.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<GetMissionResponse>> GetMissions(
        CancellationToken cancellationToken = default)
    {
        return await Mediator.Send(new GetMissionsQuery(), cancellationToken);
    }


    [HttpGet("Get-Missions-By-Id")]
    [SwaggerOperation("Get Mission by ID")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetMissionResponse> GetMissionById(Guid aggregateId,
        CancellationToken cancellationToken = default)
    {
        return await Mediator.Send(new GetMissionByIdQuery(aggregateId), cancellationToken);
    }
}