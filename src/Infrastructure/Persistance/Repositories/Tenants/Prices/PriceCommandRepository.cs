using Shifty.Application.Interfaces.Tenants.Prices;

namespace Shifty.Persistence.Repositories.Tenants.Prices;

public class PriceCommandRepository(
    ShiftyTenantDbContext db,
    IdentityService identityService,
    ILogger<PriceCommandRepository> logger,
    IStringLocalizer<PriceCommandRepository> localizer
) : IPriceCommandRepository
{
    public async Task CreateNewPrice(Price price, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Creating new active price by UserId: {UserId}", identityService.GetUserId());

            await db.Prices.Where(p => p.IsActive)
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsActive, false), cancellationToken);

            price.IsActive = true;
            price.CreatedBy = identityService.GetUserId();

            db.Prices.Add(price);
            await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("New price created with ID: {PriceId}", price.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating new price");
            throw IpaException.InternalServerError(localizer["An error occurred while creating new price."]);
        }
    }
}