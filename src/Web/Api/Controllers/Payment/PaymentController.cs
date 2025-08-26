using Shifty.Application.Base.Payment.Commands.CreatePayment;
using Shifty.Application.Base.Payment.Commands.Verify;
using Shifty.Application.Base.Payment.Queries.ListPayments;
using Shifty.Application.Base.Payment.Request.Commands.CreatePayment;
using Shifty.Application.Base.Payment.Request.Queries.ListPayment;

namespace Shifty.Api.Controllers.Payment;

public class PaymentController : IpaBaseController
{
    /// <summary>
    ///     Retrieves a list of all payments.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Returns the list of payments.</response>
    [HttpGet]
    [SwaggerOperation(Summary = "List Payments", Description = "Retrieves a list of all registered payments.")]
    [ProducesResponseType(typeof(List<PaymentQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),          StatusCodes.Status401Unauthorized)]
    public async Task<List<PaymentQueryResponse>> ListPayments(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new ListPaymentsQuery(), cancellationToken);
    }

    /// <summary>
    ///     Creates a new payment and returns the payment URL.
    /// </summary>
    /// <param name="request">The request containing payment creation data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">Payment created successfully.</response>
    /// <response code="400">Invalid payment request data.</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Create Payment", Description = "Creates a new payment and returns the redirect URL.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<string> PostPayment(
        [FromBody] CreatePaymentRequest request,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(request.Adapt<CreatePaymentCommand>(), cancellationToken);
    }

    /// <summary>
    ///     Verifies the payment status after redirect from payment gateway.
    /// </summary>
    /// <param name="authority">The payment authority code returned by the payment gateway.</param>
    /// <param name="status">The status code from the payment gateway.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">Payment verified and redirected successfully.</response>
    /// <response code="400">Missing or invalid verification data.</response>
    [HttpGet("verify")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Verify Payment",
        Description = "Verifies the payment based on query parameters from the gateway.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyPayment(
        [FromQuery] string authority,
        [FromQuery] string status,
        CancellationToken cancellationToken)
    {
        return RedirectPermanent(await Mediator.Send(new VerifyPaymentCommand(authority, status), cancellationToken));
    }
}