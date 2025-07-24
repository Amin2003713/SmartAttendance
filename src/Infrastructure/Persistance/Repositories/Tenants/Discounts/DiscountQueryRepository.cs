using DNTPersianUtils.Core;
using Shifty.Application.Discounts.Request.Queries.GetAllDiscount;
using Shifty.Application.Interfaces.Tenants.Discounts;

namespace Shifty.Persistence.Repositories.Tenants.Discounts;

public class DiscountQueryRepository(
    ShiftyTenantDbContext db,
    ILogger<DiscountQueryRepository> logger,
    IStringLocalizer<DiscountQueryRepository> localizer
) : IDiscountQueryRepository
{
    public async Task<bool> DiscountExists(string code, CancellationToken cancellationToken)
    {
        logger.LogInformation("Checking if discount with code '{Code}' exists", code);

        try
        {
            return await db.Discounts.AnyAsync(a => a.Code == code, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking existence of discount with code: {Code}", code);
            throw new InvalidOperationException(localizer["An error occurred while checking discount existence."]);
        }
    }

    public async Task<List<GetAllDiscountResponse>> GetAllDiscounts(CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching all discounts...");

        try
        {
            var result = await db.Discounts.Where(a => !a.Code!.Contains("Discount for Each 5"))
                .Include(p => p.Payments)
                .ThenInclude(p => p.Tenant)
                .Include(p => p.TenantDiscount)
                .Select(a =>
                    new GetAllDiscountResponse
                    {
                        Id = a.Id,
                        Code = $"{a.Code} ({(a.IsPrivate ? "خاص" : "عام")})",
                        DiscountType = a.DiscountType,
                        StartDate = a.StartDate,
                        EndDate = a.StartDate.AddDays(a.Duration),
                        Duration = a.Duration,
                        CreatedAt = a.CreatedAt,
                        IsActive =
                            (a.StartDate.AddDays(a.Duration) - DateTime.Now).TotalDays > 0 && a.DeletedBy == null,
                        PackageMonth = a.PackageMonth,
                        RemainingDays = (a.EndDate - DateTime.Now).ToTimeSpanParts(),
                        DiscountCompanyUsage = a.TenantDiscount.Select(q => new DiscountCompanyResponse
                            {
                                CompanyName = db.TenantInfo.Where(e => q.TenantId == e.Id)
                                    .Select(w => w.Name)
                                    .FirstOrDefault(),
                                DateOfUse = db.Payments.Where(w => w.TenantId == q.TenantId && w.DiscountId == a.Id)
                                    .Select(r => r.PaymentDate)
                                    .FirstOrDefault(),
                                IsUsed = q.IsUsed
                            })
                            .ToList()
                    })
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(cancellationToken);

            logger.LogInformation("Fetched {Count} discounts", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching all discounts.");
            throw new InvalidOperationException(localizer["An error occurred while retrieving discounts."]);
        }
    }

    public async Task<Discount> GetDiscount(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching discount with ID: {Id}", id);
        var discount = await db.Discounts.Include(a => a.TenantDiscount)
            .ThenInclude(a => a.Tenant)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (discount is null)
        {
            logger.LogWarning("Discount with ID: {Id} not found", id);
            throw new KeyNotFoundException(localizer["Discount not found."]);
        }

        return discount;
    }

    public async Task<Discount?> GetDiscount(string code, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching discount with code: {Code}", code);

        try
        {
            return await db.Discounts.FirstOrDefaultAsync(a => a.Code == code, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching discount with code: {Code}", code);
            throw new InvalidOperationException(localizer["An error occurred while retrieving the discount."]);
        }
    }

    public async Task<TenantDiscount> GetTenantDiscounts(
        Guid discountId,
        string tenantId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching TenantDiscount for TenantId: {TenantId}, DiscountId: {DiscountId}",
            tenantId,
            discountId);

        var result =
            await db.TenantDiscounts.FirstOrDefaultAsync(a => a.DiscountId == discountId && a.TenantId == tenantId,
                cancellationToken);

        if (result == null)
        {
            logger.LogWarning("TenantDiscount not found for TenantId: {TenantId}, DiscountId: {DiscountId}",
                tenantId,
                discountId);

            return null!;
        }

        return result;
    }
}