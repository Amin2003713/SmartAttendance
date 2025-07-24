using Mapster;
using Shifty.Application.Interfaces.Tenants.Prices;
using Shifty.Application.Prices.Queries.GetPrice;
using Shifty.Application.Prices.Request.Queries.GetPrice;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Prices.Queries.GetPrice;

public class GetPriceQueryHandler(
    IPriceQueryRepository priceQueryRepository,
    ILogger<GetPriceQueryHandler> logger,
    IStringLocalizer<GetPriceQueryHandler> localizer
) : IRequestHandler<GetPriceQuery, GetPriceResponse>
{
    public async Task<GetPriceResponse> Handle(GetPriceQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var price = await priceQueryRepository.GetPrice(cancellationToken);

            if (price == null)
            {
                logger.LogWarning("No active price found.");
                throw IpaException.NotFound(localizer["No price configuration found."]);
            }

            logger.LogInformation("Price retrieved successfully. ID: {Id}", price.Id);

            return price.Adapt<GetPriceResponse>();
        }
        catch (IpaException ex)
        {
            logger.LogError(ex, "Business exception occurred while retrieving price.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while retrieving price.");
            throw IpaException.InternalServerError(localizer["An unexpected error occurred while retrieving price."]);
        }
    }
}