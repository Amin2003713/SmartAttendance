using Shifty.Domain.Tenants.Payments;

namespace Shifty.Application.Discounts.Commands.UseDiscount;

public record UseDiscountCommand(
    Payments Payment
) : IRequest;