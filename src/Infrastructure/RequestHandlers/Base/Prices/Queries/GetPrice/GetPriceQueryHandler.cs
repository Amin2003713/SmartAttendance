using Mapster;
using Shifty.Application.Base.Prices.Queries.GetPrice;
using Shifty.Application.Base.Prices.Request.Queries.GetPrice;
using Shifty.Application.Interfaces.Tenants.Prices;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Base.Prices.Queries.GetPrice;

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
                throw ShiftyException.NotFound(localizer["No price configuration found."]);
            }

            logger.LogInformation("Price retrieved successfully. ID: {Id}", price.Id);

            return price.Adapt<GetPriceResponse>();
        }
        catch (ShiftyException ex)
        {
            logger.LogError(ex, "Business exception occurred while retrieving price.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while retrieving price.");
            throw ShiftyException.InternalServerError(localizer["An unexpected error occurred while retrieving price."]);
        }
    }
}