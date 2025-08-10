using Microsoft.AspNetCore.Mvc;
using Shifty.Application.Stations.Commands.Create;
using Shifty.Application.Stations.Requests.Commands.Create;
using Shifty.Domain.Stations;

namespace Shifty.Api.Controllers.Stations;

public class StationController : IpaBaseController
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a Station", Description = "Creates a new Station")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task CreateStation([FromBody] CreateStationRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateStationCommand>(), cancellationToken);
    }
}