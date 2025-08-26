using Microsoft.AspNetCore.Mvc;
using Shifty.Application.Features.Vehicles.Commands.Create;
using Shifty.Application.Features.Vehicles.Commands.Update;
using Shifty.Application.Features.Vehicles.Requests.Commands.Create;
using Shifty.Application.Features.Vehicles.Requests.Commands.Update;

namespace Shifty.Api.Controllers.Vehicles;

public class VehicleController : IpaBaseController
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a Vehicles", Description = "Creates a new Vehicles")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task CreateStation([FromBody] CreateVehicleRequest request, CancellationToken cancellationToken)
        => await Mediator.Send(request.Adapt<CreateVehicleCommand>(), cancellationToken);


    [HttpPut]
    [SwaggerOperation(Summary = "Update a Vehicle", Description = "Update a Vehicle")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task UpdateStation([FromBody] UpdateVehicleRequest request, CancellationToken cancellationToken) =>
        await Mediator.Send(request.Adapt<UpdateVehicleCommand>(), cancellationToken);


    // [HttpDelete]
    // [SwaggerOperation(Summary = "Delete a Stations", Description = "Delete a Stations")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    // public async Task DeleteStation(Guid id, CancellationToken cancellationToken) =>
    //     await Mediator.Send(new DeleteStationCommand(id), cancellationToken);
    //
    //
    // [HttpGet]
    // [SwaggerOperation(Summary = "Get Stations")]
    // [ProducesResponseType(typeof(List<GetStationResponse>), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    // public async Task<List<GetStationResponse>> GetStations(CancellationToken cancellationToken) =>
    //     await Mediator.Send(new GetStationsQuery(), cancellationToken);
    //
    // [HttpGet]
    // [SwaggerOperation(Summary = "Get By Id")]
    // [ProducesResponseType(typeof(GetStationResponse), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    // public async Task<GetStationResponse> GetById(Guid id, CancellationToken cancellationToken)
    //     => await Mediator.Send(new GetStationByIdQuery(id), cancellationToken);
}