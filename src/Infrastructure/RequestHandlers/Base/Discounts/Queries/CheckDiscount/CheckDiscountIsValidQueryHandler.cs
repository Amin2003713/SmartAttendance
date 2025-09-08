using Finbuckle.MultiTenant.Abstractions;
using Mapster;
using SmartAttendance.Application.Base.Discounts.Queries.CheckDiscount;
using SmartAttendance.Application.Base.Discounts.Request.Queries.CheckDiscount;
using SmartAttendance.Application.Interfaces.Tenants.Discounts;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Tenants;

namespace SmartAttendance.RequestHandlers.Base.Discounts.Queries.CheckDiscount;

public class CheckDiscountIsValidQueryHandler(
    IDiscountQueryRepository discountQueryRepository,
    IMultiTenantContextAccessor<SmartAttendanceTenantInfo> tenantContext,
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
            throw SmartAttendanceException.NotFound(localizer["Discount not found."].Value);
        }

        // Check if the discount has expired.
        if (DateTime.UtcNow > discount.StartDate.AddDays(discount.Duration))
        {
            logger.LogWarning("Discount with code {Code} has expired.", request.Code);
            throw SmartAttendanceException.NotFound(localizer["This discount has expired."].Value);
        }

        var tenantId       = tenantContext.MultiTenantContext.TenantInfo!.Id!;
        var tenantDiscount = await discountQueryRepository.GetTenantDiscounts(discount.Id, tenantId, cancellationToken);

        if (tenantDiscount is { IsUsed: true })
        {
            logger.LogWarning("Discount with code {Code} has already been used by tenant {TenantId}.",
                request.Code,
                tenantId);

            throw SmartAttendanceException.NotFound(localizer["Used."].Value);
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

        throw SmartAttendanceException.NotFound(localizer["Invalid."].Value);
    }
}