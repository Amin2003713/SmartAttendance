using System.Collections.Generic;
using Shifty.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace Shifty.Api.Controllers.Divisions;

public class DivisionController : BaseController
{
    /// <summary>
    ///     Creates a new division.
    /// </summary>
    /// <param name="request">The details of the division to create.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The response containing the created division details.</returns>
    /// <response code="201">Returns the created division's details.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="401">If the user is unauthorized.</response>
    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [SwaggerOperation("Create a new division.")]
    [ProducesResponseType(typeof(CreateDivisionResponse) , StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UnauthorizedResult) ,     StatusCodes.Status401Unauthorized)]
    public async Task<CreateDivisionResponse> CreateDivision(
        [FromBody] CreateDivisionRequest request ,
        CancellationToken cancellationToken)
    {
       return await Mediator.Send(request.Adapt<CreateDivisionCommand>() , cancellationToken);
    }

    /// <summary>
    ///     Retrieves detailed information about a division by its identifier.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>The response containing the division's detailed information.</returns>
    /// <response code="200">Returns the division's detailed information.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="401">If the user is unauthorized.</response>
    /// <response code="404">If the division with the specified identifier is not found.</response>
    [HttpGet]
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [SwaggerOperation("Retrieve detailed information about a division by its identifier.")]
    [ProducesResponseType(typeof(GetDivisionResponse) , StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResult) ,  StatusCodes.Status401Unauthorized)]
    public async Task<List<GetDivisionResponse>> GetDivisionById(
        CancellationToken cancellationToken)
        => await Mediator.Send(new GetDivisionQuery() , cancellationToken);
}