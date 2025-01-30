using System.Collections.Generic;
using Shifty.Application.Divisions.Command.Create;
using Shifty.Application.Divisions.Queries.GetDefault;
using Shifty.Application.Divisions.Requests.Create;
using Shifty.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace Shifty.Api.Controllers.Divisions;

public class DivisionController : BaseController
{
   
    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [SwaggerOperation("Create a new division.")]
     [ProducesResponseType(typeof(CreateDivisionResponse) , StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UnauthorizedResult) ,     StatusCodes.Status401Unauthorized)]
    public async Task<CreateDivisionResponse> CreateDivision([FromBody] CreateDivisionRequest request , CancellationToken cancellationToken)
    {
       return await Mediator.Send(request.Adapt<CreateDivisionCommand>() , cancellationToken);
    }

    [HttpGet]
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [SwaggerOperation("Retrieve detailed information about a division by its identifier.")]
    [ProducesResponseType(typeof(List<GetDivisionResponse>) , StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResult) ,        StatusCodes.Status401Unauthorized)]
    public async Task<List<GetDivisionResponse>> ListDivision(
        CancellationToken cancellationToken)
        => await Mediator.Send(new GetDivisionQuery() , cancellationToken);
}