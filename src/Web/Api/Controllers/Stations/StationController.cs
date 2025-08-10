using Shifty.Application.Stations.Commands.Create;
using Shifty.Application.Stations.Commands.Delete;
using Shifty.Application.Stations.Commands.Update;
using Shifty.Application.Stations.Queries.GetStations;
using Shifty.Application.Stations.Requests.Commands.Create;
using Shifty.Application.Stations.Requests.Commands.Update;
using Shifty.Application.Stations.Requests.Queries.GetStations;

namespace Shifty.Api.Controllers.Stations;

public class StationController : IpaBaseController
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a Stations", Description = "Creates a new Stations")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task CreateStation([FromBody] CreateStationRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateStationCommand>(), cancellationToken);
    }

    [HttpPut]
    [SwaggerOperation(Summary = "Update a Stations", Description = "Update a Stations")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task UpdateStation([FromBody] UpdateStationRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateStationCommand>(), cancellationToken);
    }


    [HttpDelete]
    [SwaggerOperation(Summary = "Delete a Stations", Description = "Delete a Stations")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task DeleteStation(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteStationCommand(id), cancellationToken);
    }


    [HttpGet]
    [SwaggerOperation(Summary = "Get Stations")]
    [ProducesResponseType(typeof(List<GetStationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<List<GetStationResponse>> GetStations(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetStationsQuery(), cancellationToken);
    }
}