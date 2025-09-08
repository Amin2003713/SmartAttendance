using SmartAttendance.Application.Base.Discounts.Commands.CreateDiscount;
using SmartAttendance.Application.Base.Discounts.Commands.DeleteDiscount;
using SmartAttendance.Application.Base.Discounts.Queries.CheckDiscount;
using SmartAttendance.Application.Base.Discounts.Queries.GetAllDiscount;
using SmartAttendance.Application.Base.Discounts.Request.Commands.CreateDisCount;
using SmartAttendance.Application.Base.Discounts.Request.Queries.CheckDiscount;
using SmartAttendance.Application.Base.Discounts.Request.Queries.GetAllDiscount;

namespace SmartAttendance.Api.Controllers.Discounts;

public class DiscountController : SmartAttendanceBaseController
{
    /// <summary>
    ///     Creates a new discount.
    /// </summary>
    /// <param name="request">The request payload containing discount data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">Successfully created the discount.</response>
    /// <response code="400">Invalid input data.</response>
    [HttpPost]
    [SwaggerOperation("Creates a Discount.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task PostDiscount([FromBody] CreateDiscountRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateDiscountCommand>(), cancellationToken);
    }

    /// <summary>
    ///     Validates if the given discount code is valid for a company based on the selected package duration.
    /// </summary>
    /// <param name="code">The discount code to validate.</param>
    /// <param name="packageMonth">The number of months in the selected package.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of discount validation.</returns>
    /// <response code="200">Discount is valid or invalid based on input.</response>
    /// <response code="400">If input parameters are invalid.</response>
    [HttpGet("Validate-Discount")]
    [SwaggerOperation("Validate Discount.")]
    [ProducesResponseType(typeof(CheckDiscountIsValidResponse), StatusCodes.Status200OK)]
    public async Task<CheckDiscountIsValidResponse> CheckIfDiscountCodeIsValidForCompany(
        string code,
        int packageMonth,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(CheckDiscountIsValidQuery.Create(code, packageMonth), cancellationToken);
    }

    /// <summary>
    ///     Retrieves all available discounts.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of all discounts.</returns>
    /// <response code="200">Successfully retrieved the list of discounts.</response>
    [HttpGet("Get-All-Discounts")]
    [SwaggerOperation("GetAll Discount.")]
    [ProducesResponseType(typeof(List<GetAllDiscountResponse>), StatusCodes.Status200OK)]
    public async Task<List<GetAllDiscountResponse>> GetAllDiscounts(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetAllDiscountQuery(), cancellationToken);
    }

    /// <summary>
    ///     Deletes a discount by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the discount.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">Successfully deleted the discount.</response>
    /// <response code="400">If the ID is invalid or missing.</response>
    /// <response code="404">If the discount with the specified ID is not found.</response>
    [HttpDelete]
    [SwaggerOperation("Delete Discount.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task DeleteDiscount(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteDiscountCommand(id), cancellationToken);
    }
}