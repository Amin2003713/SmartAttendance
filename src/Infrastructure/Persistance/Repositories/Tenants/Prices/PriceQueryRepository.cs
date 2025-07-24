using Shifty.Application.Interfaces.Tenants.Prices;
using Shifty.Domain.Tenants;

namespace Shifty.Persistence.Repositories.Tenants.Prices;

public class PriceQueryRepository(
    ShiftyTenantDbContext db,
    ILogger<PriceQueryRepository> logger,
    IStringLocalizer<PriceQueryRepository> localizer
) : IPriceQueryRepository
{
    public async Task<Price> GetPrice(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Retrieving active price entry.");
            var price = await db.Prices.SingleOrDefaultAsync(a => a.IsActive, cancellationToken);

            if (price is null)
            {
                logger.LogWarning("No active price found.");
                throw new InvalidOperationException(localizer["No active price available."]);
            }

            return price;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving active price entry.");
            throw new InvalidOperationException(localizer["An error occurred while retrieving the active price."]);
        }
    }

    public async Task<Price> GetPriceById(Guid? id, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Retrieving price by ID: {PriceId}", id);
            var price = await db.Prices.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (price is null)
            {
                logger.LogWarning("Price not found with ID: {PriceId}", id);
                throw new InvalidOperationException(localizer["Price not found."]);
            }

            return price;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving price by ID: {PriceId}", id);
            throw new InvalidOperationException(localizer["An error occurred while retrieving the price by ID."]);
        }
    }
}