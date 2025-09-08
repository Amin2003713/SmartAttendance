using SmartAttendance.Application.Base.Discounts.Commands.UseDiscount;
using SmartAttendance.Application.Interfaces.Tenants.Discounts;
using SmartAttendance.Common.General.Enums.Discount;

namespace SmartAttendance.RequestHandlers.Base.Discounts.Commands.UseDiscount;

public record UseDiscountCommandHandler(
    IDiscountQueryRepository QueryRepository,
    IDiscountCommandRepository CommandRepository,
    ILogger<UseDiscountCommandHandler> Logger
)
    : IRequestHandler<UseDiscountCommand>
{
    public async Task Handle(UseDiscountCommand request, CancellationToken cancellationToken)
    {
        if (request.Payment.DiscountId is null)
        {
            Logger.LogDebug("No discount ID provided in the payment. Skipping discount application.");
            return;
        }

        var discount = await QueryRepository.GetDiscount(request.Payment.DiscountId.Value, cancellationToken);

        if (discount == null)
        {
            Logger.LogWarning("Discount with ID {DiscountId} not found. Skipping discount application.",
                request.Payment.DiscountId.Value);

            return;
        }

        var tenantDiscount = discount.TenantDiscount.FirstOrDefault(a => a.TenantId == request.Payment.TenantId);

        if (tenantDiscount is null)
        {
            Logger.LogWarning("Tenant discount for tenant ID {TenantId} not found in discount {DiscountId}.",
                request.Payment.TenantId,
                discount.Id);

            return;
        }

        if (discount.DiscountType == DiscountType.ExtraDays)
        {
            request.Payment.EndDate = request.Payment.EndDate.AddDays((double)discount.Value);
            Logger.LogInformation(
                "Applied extra days discount: added {ExtraDays} days to payment end date. New EndDate: {EndDate}.",
                discount.Value,
                request.Payment.EndDate);
        }
        else
        {
            Logger.LogInformation("Discount type {DiscountType} does not affect payment end date.",
                discount.DiscountType);
        }

        tenantDiscount.IsUsed = true;
        await CommandRepository.UpdateTenantDiscount(tenantDiscount, cancellationToken);
        Logger.LogInformation("Tenant discount for tenant ID {TenantId} in discount {DiscountId} marked as used.",
            request.Payment.TenantId,
            discount.Id);
    }
}