using SmartAttendance.Application.Base.Companies.Commands.InitialCompany;
using SmartAttendance.Application.Base.Companies.Queries.CheckDomain;
using SmartAttendance.Application.Base.Companies.Requests.InitialCompany;
using SmartAttendance.Application.Features.Users.Queries.GetUserTenants;

namespace SmartAttendance.Api.Controllers.Panels;

public class PanelController : SmartAttendanceBaseController
{
    /// <summary>
    ///     Checks the availability or status of a given domain.
    /// </summary>
    /// <param name="domain">The domain name to check.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// Returns a
    /// <see cref="CheckDomainResponse" />
    /// containing domain status if the request is successful (200 OK),
    /// or an
    /// <see cref="ApiProblemDetails" />
    /// object if there is a bad request (400 Bad Requests).
    [HttpGet("check-domain")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CheckDomainResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),   StatusCodes.Status400BadRequest)]
    public virtual async Task<CheckDomainResponse> CheckDomain(
        [FromQuery] string domain,
        CancellationToken  cancellationToken)
    {
        return await Mediator.Send(new CheckDomainQuery(domain), cancellationToken);
    }


    /// <summary>
    ///     Retrieves the list of tenants associated with a given user.
    /// </summary>
    /// <param name="userName">The user name to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of tenants for the provided user.</returns>
    [HttpGet("user-tenants")]
    [ProducesResponseType(typeof(List<GetUserTenantResponse>), 200)]
    [ProducesResponseType(typeof(string),                      400)]
    public async Task<List<GetUserTenantResponse>> GetUserTenants(
        [FromQuery] string userName,
        CancellationToken  cancellationToken)
    {
        return await Mediator.Send(new GetUserTenantQuery(userName), cancellationToken);
    }

    /// <summary>
    ///     Creates a new company (tenant).
    /// </summary>
    /// <param name="request">The request containing the details of the company to create.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The response containing the created company details.</returns>
    /// <response code="200">Returns the created company details.</response>
    /// <response code="400">If the request is invalid or the company could not be created.</response>
    [HttpPost("initialize-company")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string),            StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public virtual async Task<string> InitialCompany(
        [FromBody] InitialCompanyRequest request,
        CancellationToken                cancellationToken)
    {
        return await Mediator.Send(request.Adapt<InitialCompanyCommand>(), cancellationToken);
    }
}