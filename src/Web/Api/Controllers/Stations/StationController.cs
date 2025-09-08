using SmartAttendance.Application.Features.Stations.Commands.Create;
using SmartAttendance.Application.Features.Stations.Commands.Delete;
using SmartAttendance.Application.Features.Stations.Commands.Update;
using SmartAttendance.Application.Features.Stations.Queries.ById;
using SmartAttendance.Application.Features.Stations.Queries.GetStations;
using SmartAttendance.Application.Features.Stations.Requests.Commands.Create;
using SmartAttendance.Application.Features.Stations.Requests.Commands.Update;
using SmartAttendance.Application.Features.Stations.Requests.Queries.GetStations;

namespace SmartAttendance.Api.Controllers.Stations;

public class StationController : SmartAttendanceBaseController
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
    [ProducesResponseType(typeof(ApiProblemDetails),        StatusCodes.Status400BadRequest)]
    public async Task<List<GetStationResponse>> GetStations(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetStationsQuery(), cancellationToken);
    }

    [HttpGet("By-Id")]
    [SwaggerOperation(Summary = "Get By Id")]
    [ProducesResponseType(typeof(GetStationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),  StatusCodes.Status400BadRequest)]
    public async Task<GetStationResponse> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetStationByIdQuery(id), cancellationToken);
    }
}