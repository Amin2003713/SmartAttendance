using Finbuckle.MultiTenant.Abstractions;
using Mapster;
using Shifty.Application.Discounts.Queries.CheckDiscount;
using Shifty.Application.Discounts.Request.Queries.CheckDiscount;
using Shifty.Application.Interfaces.Tenants.Discounts;
using Shifty.Common.Exceptions;
using Shifty.Domain.Tenants;

namespace Shifty.RequestHandlers.Discounts.Queries.CheckDiscount;

public class CheckDiscountIsValidQueryHandler(
    IDiscountQueryRepository discountQueryRepository,
    IMultiTenantContextAccessor<ShiftyTenantInfo> tenantContext,
    ILogger<CheckDiscountIsValidQueryHandler> logger,
    IStringLocalizer<CheckDiscountIsValidQueryHandler> localizer
)
    : IRequestHandler<CheckDiscountIsValidQuery, CheckDiscountIsValidResponse>
{
    public async Task<CheckDiscountIsValidResponse> Handle(
        CheckDiscountIsValidQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Checking validity of discount with code: {Code} for tenant: {TenantId}",
            request.Code,
            tenantContext.MultiTenantContext.TenantInfo?.Id);

        var discount = await discountQueryRepository.GetDiscount(request.Code, cancellationToken);

        if (discount == null)
        {
            logger.LogWarning("Discount with code {Code} not found.", request.Code);
            throw IpaException.NotFound(localizer["Discount not found."].Value);
        }

        // Check if the discount has expired.
        if (DateTime.Now > discount.StartDate.AddDays(discount.Duration))
        {
            logger.LogWarning("Discount with code {Code} has expired.", request.Code);
            throw IpaException.NotFound(localizer["This discount has expired."].Value);
        }

        var tenantId       = tenantContext.MultiTenantContext.TenantInfo!.Id!;
        var tenantDiscount = await discountQueryRepository.GetTenantDiscounts(discount.Id, tenantId, cancellationToken);

        if (tenantDiscount is { IsUsed: true })
        {
            logger.LogWarning("Discount with code {Code} has already been used by tenant {TenantId}.",
                request.Code,
                tenantId);

            throw IpaException.NotFound(localizer["Used."].Value);
        }

        // Check if the discount is applicable to the given PackageMonth.
        if (discount.PackageMonth == 0 || discount.PackageMonth == request.PackageMonth)
        {
            logger.LogInformation("Discount with code {Code} is valid for PackageMonth {PackageMonth}.",
                request.Code,
                request.PackageMonth);

            return discount.Adapt<CheckDiscountIsValidResponse>();
        }

        logger.LogWarning("Discount with code {Code} is not valid for PackageMonth {PackageMonth}.",
            request.Code,
            request.PackageMonth);

        throw IpaException.NotFound(localizer["Invalid."].Value);
    }
}