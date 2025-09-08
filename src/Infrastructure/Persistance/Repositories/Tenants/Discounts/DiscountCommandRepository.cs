using SmartAttendance.Application.Interfaces.Tenants.Discounts;

namespace SmartAttendance.Persistence.Repositories.Tenants.Discounts;

public class DiscountCommandRepository(
    SmartAttendanceTenantDbContext db,
    IdentityService identityService,
    ILogger<DiscountCommandRepository> logger,
    IStringLocalizer<DiscountCommandRepository> localizer
) : IDiscountCommandRepository
{
    public async Task Add(Discount discount, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Adding discount {DiscountId}", discount.Id);

            db.Discounts.Add(discount);
            await db.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Discount added with ID: {DiscountId}", discount.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding discount: {DiscountId}", discount.Id);
            throw SmartAttendanceException.InternalServerError(localizer["An error occurred while adding the discount."]);
        }
    }

    public async Task Delete(Discount discount, CancellationToken cancellationToken)
    {
        try
        {
            var userId = identityService.GetUserId();
            discount.DeletedBy = userId;
            discount.DeletedAt = DateTime.UtcNow;
            discount.IsActive = false;

            logger.LogInformation("Deleting (soft) discount with ID: {DiscountId} by UserId: {UserId}",
                discount.Id,
                userId);

            db.Discounts.Update(discount);
            await db.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Discount marked as deleted.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting discount with ID: {DiscountId}", discount.Id);
            throw SmartAttendanceException.InternalServerError(localizer["An error occurred while deleting the discount."]);
        }
    }

    public async Task UpdateTenantDiscount(TenantDiscount tenantDiscount, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Updating TenantDiscount for TenantId: {TenantId}", tenantDiscount.TenantId);

            db.TenantDiscounts.Update(tenantDiscount);
            await db.SaveChangesAsync(cancellationToken);

            logger.LogInformation("TenantDiscount updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error while updating TenantDiscount for TenantId: {TenantId}",
                tenantDiscount.TenantId);

            throw SmartAttendanceException.InternalServerError(localizer["An error occurred while updating tenant discount."]);
        }
    }
}