using Shifty.Application.Pdf.Query.GetFactorPdf;

namespace Shifty.Api.Controllers.Pdf;

public class PdfController : IpaBaseController
{
    /// <summary>
    ///     Generates and retrieves the PDF file for a specific payment (factor).
    /// </summary>
    /// <param name="paymentId">The identifier of the payment.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Factor PDF generated and returned successfully.</response>
    /// <response code="400">Invalid payment ID provided.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpGet("Get-Factor-Pdf")]
    [ProducesResponseType(typeof(string),            StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    // [Authorize(RoleTypes = $"{nameof(RoleTypes.Admin)}")]
    public async Task<string> FactorPdf(Guid paymentId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetFactorPdfQuery(paymentId), cancellationToken);
    }
}