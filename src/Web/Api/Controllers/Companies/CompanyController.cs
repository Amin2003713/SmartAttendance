using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shifty.Api.Controllers.Common;
using Shifty.ApiFramework.Tools;
using Shifty.Application.Companies.Command.InitialCompany;
using Shifty.Application.Companies.Queries.CheckDomain;
using Shifty.Application.Companies.Queries.GetCompanyInfo;
using Shifty.Application.Companies.Requests;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Api.Controllers.Companies
{
    public class CompanyController : BaseController
    {
        /// <summary>
        ///     Creates a new company (tenant).
        /// </summary>
        /// <param name="request">The request containing the details of the company to create.</param>
        /// <returns>The response containing the created company details.</returns>
        /// <response code="200">Returns the created company details.</response>
        /// <response code="400">If the request is invalid or the company could not be created.</response>
        [HttpPost("Panel/initialize-company")]
        [SwaggerOperation("""
                          create the Tenant Admin With Full privileges 
                          Create a new company (tenant)
                          initiate a new DataBase For the Company
                          At least Send Activation Sms
                          """)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string) ,            StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiProblemDetails) , StatusCodes.Status400BadRequest)]
        public virtual async Task<string> InitialCompany([FromBody] InitialCompanyRequest request , CancellationToken cancellationToken)
        {
            return await Mediator.Send(request.Adapt<InitialCompanyCommand>() , cancellationToken);
        }


        /// <summary>
        ///     Retrieves detailed information about a company by its identifier.
        /// </summary>
        /// <param name="domain">The unique identifier of the company.</param>
        /// <returns>The response containing the company's detailed information.</returns>
        /// <response code="200">Returns the company's detailed information.</response>
        /// <response code="400">If the request is invalid (e.g., the identifier is missing or incorrect).</response>
        /// <response code="404">If the company with the specified identifier is not found.</response>
        [HttpGet("get-info")]
        [SwaggerOperation("Retrieve detailed information about a company by its identifier.")]
        [ProducesResponseType(typeof(GetCompanyInfoResponse) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedResult) ,     StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BadRequestResult) ,       StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResult) ,         StatusCodes.Status404NotFound)]
        public virtual async Task<GetCompanyInfoResponse> GetCompanyInfo([FromQuery] string domain , CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetCompanyInfoQuery(domain) , cancellationToken);
        }


        [HttpGet("panel/check-domain")]
        [SwaggerOperation("Retrieve detailed information about a company by its identifier.")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CheckDomainResponse) , StatusCodes.Status200OK)]
        public virtual async Task<CheckDomainResponse> CheckDomain([FromQuery] string domain , CancellationToken cancellationToken)
        {
            return await Mediator.Send(new CheckDomainQuery(domain) , cancellationToken);
        }
    }
}