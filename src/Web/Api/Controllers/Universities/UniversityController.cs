using SmartAttendance.Application.Base.Universities.Commands.UpdateUniversity;
using SmartAttendance.Application.Base.Universities.Queries.GetUniversityInfo;
using SmartAttendance.Application.Base.Universities.Requests.UpdateCompany;
using SmartAttendance.Application.Base.Universities.Responses.GetCompanyInfo;

namespace SmartAttendance.Api.Controllers.Universities;

public class UniversityController : SmartAttendanceBaseController
{
    /// <summary>
    ///     Retrieves detailed information about a University by its identifier.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>The response containing the University's detailed information.</returns>
    /// <response code="200">Returns the University's detailed information.</response>
    /// <response code="400">If the request is invalid (e.g., the identifier is missing or incorrect).</response>
    /// <response code="404">If the University with the specified identifier is not found.</response>
    [HttpGet("get-info")]
    [AllowAnonymous]
    [SwaggerOperation("Retrieve detailed information about a University by its identifier.")]
    [ProducesResponseType(typeof(GetUniversityInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResult),        StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult),            StatusCodes.Status404NotFound)]
    public virtual async Task<GetUniversityInfoResponse> GetUniversityInfo(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetUniversityInfoQuery(TenantContext.TenantInfo?.Identifier!), cancellationToken);
    }


    /// <summary>
    ///     Updates the details of a University.
    /// </summary>
    /// <param name="request">The update request containing the new University details.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>An HTTP response indicating the outcome of the update operation.</returns>
    /// <response code="204">If the University details are successfully updated.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="401">If the user is unauthorized.</response>
    /// <response code="404">If the University with the specified identifier is not found.</response>
    [HttpPut("update")]
    [SwaggerOperation("Update the details of a University.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(UnauthorizedResult),     StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult),         StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
    public virtual async Task UpdateUniversity(
        [FromForm] UpdateUniversityRequest request,
        CancellationToken               cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateUniversityCommand>().AddMedia(request.Logo),
            cancellationToken);
    }
}