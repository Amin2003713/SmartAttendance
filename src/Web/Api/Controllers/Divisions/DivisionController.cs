using System.Collections.Generic;
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
    [SwaggerOperation("Create a new division.")]
    [ProducesResponseType(typeof(CreateDivisionResponse) , StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BadRequestResult) ,       StatusCodes.Status400BadRequest)]
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
    [SwaggerOperation("Retrieve detailed information about a division by its identifier.")]
    [ProducesResponseType(typeof(GetDivisionResponse) , StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult) ,    StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResult) ,  StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult) ,      StatusCodes.Status404NotFound)]
    public async Task<List<GetDivisionResponse>> GetDivisionById(
        CancellationToken cancellationToken)
        => await Mediator.Send(new GetDivisionQuery() , cancellationToken);
}