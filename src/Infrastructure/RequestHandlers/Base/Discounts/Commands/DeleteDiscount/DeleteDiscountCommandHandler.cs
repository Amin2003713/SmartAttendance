using Shifty.Application.Base.Discounts.Commands.DeleteDiscount;
using Shifty.Application.Interfaces.Tenants.Discounts;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Base.Discounts.Commands.DeleteDiscount;

public class DeleteDiscountCommandHandler(
    IDiscountQueryRepository discountQueryRepository,
    IDiscountCommandRepository discountCommandRepository,
    ILogger<DeleteDiscountCommandHandler> logger,
    IStringLocalizer<DeleteDiscountCommandHandler> localizer
)
    : IRequestHandler<DeleteDiscountCommand>
{
    public async Task Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to delete discount with Id {DiscountId}.", request.Id);

        var discount = await discountQueryRepository.GetDiscount(request.Id, cancellationToken);

        if (discount == null)
        {
            logger.LogWarning("Discount with Id {DiscountId} not found.", request.Id);
            throw ShiftyException.NotFound(localizer["Discount not found."].Value);
        }

        await discountCommandRepository.Delete(discount, cancellationToken);
        logger.LogInformation("Successfully deleted discount with Id {DiscountId}.", request.Id);
    }
}