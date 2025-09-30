using SmartAttendance.Application.Base.Universities.Commands.InitialUniversity;
using SmartAttendance.Application.Base.Universities.Queries.CheckDomain;
using SmartAttendance.Application.Base.Universities.Queries.ListAvailableUniversities;
using SmartAttendance.Application.Base.Universities.Requests.InitialUniversity;
using SmartAttendance.Application.Base.Universities.Responses.GetCompanyInfo;
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
    ///     Creates a new University (tenant).
    /// </summary>
    /// <param name="request">The request containing the details of the University to create.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The response containing the created University details.</returns>
    /// <response code="200">Returns the created University details.</response>
    /// <response code="400">If the request is invalid or the University could not be created.</response>
    [HttpPost("initialize-University")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string),            StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public virtual async Task<string> InitialUniversity(
        [FromForm] InitialUniversityRequest request,
        CancellationToken                cancellationToken)
    {
        return await Mediator.Send(request.Adapt<InitialUniversityCommand>(), cancellationToken);
    }

    /// <summary>
    ///     Creates a new University (tenant).
    /// </summary>
    /// <param name="request">The request containing the details of the University to create.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The response containing the created University details.</returns>
    /// <response code="200">Returns the created University details.</response>
    /// <response code="400">If the request is invalid or the University could not be created.</response>
    [HttpGet("get-Tenants")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string),            StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public virtual async Task<List<GetUniversityInfoResponse>> ListAvailableUniversities(CancellationToken                cancellationToken)
    {
        return await Mediator.Send(new ListAvailableUniversitiesQuery(), cancellationToken);
    }
}