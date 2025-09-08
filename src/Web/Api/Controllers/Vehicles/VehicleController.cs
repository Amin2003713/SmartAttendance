using SmartAttendance.Application.Features.Vehicles.Commands.Create;
using SmartAttendance.Application.Features.Vehicles.Commands.Delete;
using SmartAttendance.Application.Features.Vehicles.Commands.Update;
using SmartAttendance.Application.Features.Vehicles.Queries.GetById;
using SmartAttendance.Application.Features.Vehicles.Queries.GetVehicles;
using SmartAttendance.Application.Features.Vehicles.Requests.Commands.Create;
using SmartAttendance.Application.Features.Vehicles.Requests.Commands.Update;
using SmartAttendance.Application.Features.Vehicles.Requests.Queries.GetVehicles;

namespace SmartAttendance.Api.Controllers.Vehicles;

public class VehicleController : SmartAttendanceBaseController
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a Vehicles", Description = "Creates a new Vehicles")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task CreateStation([FromBody] CreateVehicleRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateVehicleCommand>(), cancellationToken);
    }


    [HttpPut]
    [SwaggerOperation(Summary = "Update a Vehicle", Description = "Update a Vehicle")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task UpdateStation([FromBody] UpdateVehicleRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateVehicleCommand>(), cancellationToken);
    }


    [HttpDelete]
    [SwaggerOperation(Summary = "Delete a Vehicles", Description = "Delete a Vehicles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task DeleteVehicle(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteVehicleCommand(id), cancellationToken);
    }


    [HttpGet]
    [SwaggerOperation(Summary = "Get-Vehicles")]
    [ProducesResponseType(typeof(List<GetVehicleQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),             StatusCodes.Status400BadRequest)]
    public async Task<List<GetVehicleQueryResponse>> GetVehicles(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetVehiclesQuery(), cancellationToken);
    }

    [HttpGet("By-Id")]
    [SwaggerOperation(Summary = "Get-By-Id")]
    [ProducesResponseType(typeof(GetVehicleQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),       StatusCodes.Status400BadRequest)]
    public async Task<GetVehicleQueryResponse> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetVehicleByIdQuery(id), cancellationToken);
    }
}