using Shifty.Application.Panel.Companies.Command.InitialCompany;
using Shifty.Application.Panel.Companies.Queries.CheckDomain;
using Shifty.Application.Panel.Companies.Requests;

namespace Shifty.Api.Controllers.Panels
{
    public class PanelController   : BaseController
    {

        /// <summary>
        /// Checks the availability or status of a given domain.
        /// </summary>
        /// <param name="domain">The domain name to check.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// Returns a <see cref="CheckDomainResponse"/> containing domain status if the request is successful (200 OK),
        /// or an <see cref="ApiProblemDetails"/> object if there is a bad request (400 Bad Request).
        [HttpGet("check-domain")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CheckDomainResponse) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiProblemDetails) , StatusCodes.Status400BadRequest)]
        public virtual async Task<CheckDomainResponse> CheckDomain([FromQuery] string domain , CancellationToken cancellationToken)
            => await Mediator.Send(new CheckDomainQuery(domain) , cancellationToken);

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
        [ProducesResponseType(typeof(string) ,            StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiProblemDetails) , StatusCodes.Status400BadRequest)]
        public virtual async Task<string> InitialCompany([FromBody] InitialCompanyRequest request , CancellationToken cancellationToken)
            => await Mediator.Send(request.Adapt<InitialCompanyCommand>() , cancellationToken);
    }
}