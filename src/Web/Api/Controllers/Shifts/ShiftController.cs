using Shifty.Application.Shifts.Queries.GetDefault;
using Shifty.Domain.Enums;
using System.Collections.Generic;
using Shifty.Application.Shifts.Command.Create;
using Shifty.Application.Shifts.Requests.Create;
using Swashbuckle.AspNetCore.Annotations;

namespace Shifty.Api.Controllers.Shifts
{
    public class ShiftController : BaseController
    {
        /// <summary>
        /// Creates a new shift in the system.
        /// </summary>
        /// <param name="response">The request object containing the details of the shift to be created.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A response object containing details of the created shift.</returns>
        /// <response code="200">Returns the details of the successfully created shift.</response>
        /// <response code="400">If the request is invalid (e.g., missing or incorrect fields).</response>
        /// <response code="500">If an unexpected error occurs during processing.</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new shift" ,
            Description = "Allows administrators to create a new shift by providing the necessary details such as name, timings, and grace periods."
        )]
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [ProducesResponseType(typeof(CreateShiftResponse) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,   StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,   StatusCodes.Status500InternalServerError)]
        public virtual async Task<CreateShiftResponse> CheckDomain([FromBody] CreateShiftRequest response , CancellationToken cancellationToken)
        {
            return await Mediator.Send(response.Adapt<CreateShiftCommand>() , cancellationToken);
        }


        /// <summary>
        /// gets shifts in the system.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>A response object containing details of the created shift.</returns>
        /// <response code="200">Returns the details of the successfully created shift.</response>
        /// <response code="400">If the request is invalid (e.g., missing or incorrect fields).</response>
        /// <response code="500">If an unexpected error occurs during processing.</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "get all the shift in system" ,
            Description = "Allows administrators to get  shifts"
        )]
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [ProducesResponseType(typeof(List<GetShiftsQueryResponse>) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,            StatusCodes.Status400BadRequest)]
        public virtual async Task<List<GetShiftsQueryResponse>> CheckDomain(CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetShiftsQuery() , cancellationToken);
        }
    }
}