using SmartAttendance.Application.Base.Companies.Commands.UpdateCompany;
using SmartAttendance.Application.Base.Companies.Queries.GetCompanyInfo;
using SmartAttendance.Application.Base.Companies.Requests.UpdateCompany;
using SmartAttendance.Application.Base.Companies.Responses.GetCompanyInfo;

namespace SmartAttendance.Api.Controllers.Companies;

public class CompanyController : SmartAttendanceBaseController
{
    /// <summary>
    ///     Retrieves detailed information about a company by its identifier.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>The response containing the company's detailed information.</returns>
    /// <response code="200">Returns the company's detailed information.</response>
    /// <response code="400">If the request is invalid (e.g., the identifier is missing or incorrect).</response>
    /// <response code="404">If the company with the specified identifier is not found.</response>
    [HttpGet("get-info")]
    [SwaggerOperation("Retrieve detailed information about a company by its identifier.")]
    [ProducesResponseType(typeof(GetCompanyInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResult),     StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult),         StatusCodes.Status404NotFound)]
    public virtual async Task<GetCompanyInfoResponse> GetCompanyInfo(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetCompanyInfoQuery(TenantContext.TenantInfo?.Identifier!), cancellationToken);
    }


    /// <summary>
    ///     Updates the details of a company.
    /// </summary>
    /// <param name="request">The update request containing the new company details.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>An HTTP response indicating the outcome of the update operation.</returns>
    /// <response code="204">If the company details are successfully updated.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="401">If the user is unauthorized.</response>
    /// <response code="404">If the company with the specified identifier is not found.</response>
    [HttpPut("update")]
    [SwaggerOperation("Update the details of a company.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(UnauthorizedResult),     StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult),         StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
    public virtual async Task UpdateCompany(
        [FromForm] UpdateCompanyRequest request,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateCompanyCommand>().AddMedia(request.Logo),
            cancellationToken);
    }
}