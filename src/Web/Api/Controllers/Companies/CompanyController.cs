using Shifty.Application.Companies.Responses.GetCompanyInfo;
using Swashbuckle.AspNetCore.Annotations;

namespace Shifty.Api.Controllers.Companies;

public class CompanyController : BaseController
{
    /// <summary>
    ///     Retrieves detailed information about a company by its identifier.
    /// </summary>
    /// <param name="domain">The unique identifier of the company.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The response containing the company's detailed information.</returns>
    /// <response code="200">Returns the company's detailed information.</response>
    /// <response code="400">If the request is invalid (e.g., the identifier is missing or incorrect).</response>
    /// <response code="404">If the company with the specified identifier is not found.</response>
    [HttpGet("get-info")]
    [SwaggerOperation("Retrieve detailed information about a company by its identifier.")]
    [ProducesResponseType(typeof(GetCompanyInfoResponse) , StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResult) ,     StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResult) ,         StatusCodes.Status404NotFound)]
    public virtual async Task<GetCompanyInfoResponse> GetCompanyInfo(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetCompanyInfoQuery(TenantContext.TenantInfo?.Identifier!) , cancellationToken);
    }
}