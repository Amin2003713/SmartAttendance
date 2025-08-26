using Shifty.Domain.Tenants.Payments;

namespace Shifty.Application.Base.Discounts.Commands.UseDiscount;

public record UseDiscountCommand(
    Payments Payment
) : IRequest;