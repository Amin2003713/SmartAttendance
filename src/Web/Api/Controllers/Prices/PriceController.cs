using Shifty.Application.Base.Prices.Commands.CreatePrice;
using Shifty.Application.Base.Prices.Queries.GetPrice;
using Shifty.Application.Base.Prices.Request.Commands.CreatePrice;
using Shifty.Application.Base.Prices.Request.Queries.GetPrice;

namespace Shifty.Api.Controllers.Prices;

public class PriceController : IpaBaseController
{
    /// <summary>
    ///     Retrieves the current pricing details.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The current price details.</returns>
    /// <response code="200">Returns the current price information.</response>
    /// <response code="400">If the request parameters are invalid.</response>
    /// <response code="401">If the user is unauthorized.</response>
    [HttpGet]
    [SwaggerOperation(Summary = "Get Price", Description = "Retrieves the current pricing details.")]
    [ProducesResponseType(typeof(GetPriceResponse),  StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<GetPriceResponse> GetPrice(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetPriceQuery(), cancellationToken);
    }

    /// <summary>
    ///     Creates a new price configuration.
    /// </summary>
    /// <param name="request">The request payload containing price configuration.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="201">Price created successfully.</response>
    /// <response code="400">Invalid price data provided.</response>
    /// <response code="401">User is unauthorized to perform this action.</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Create Price", Description = "Creates a new price configuration.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task CreatePrice([FromBody] CreatePriceRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreatePriceCommand>(), cancellationToken);
    }
}