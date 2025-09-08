using SmartAttendance.Domain.Tenants.Payments;

namespace SmartAttendance.Application.Base.Discounts.Commands.UseDiscount;

public record UseDiscountCommand(
    Payments Payment
) : IRequest;