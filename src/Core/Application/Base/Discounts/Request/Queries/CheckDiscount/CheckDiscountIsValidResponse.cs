using SmartAttendance.Common.General.Enums.Discount;

namespace SmartAttendance.Application.Base.Discounts.Request.Queries.CheckDiscount;

public class CheckDiscountIsValidResponse
{
    public DiscountType DiscountType { get; set; }

    public decimal Value { get; set; }

    public Guid Id { get; set; }
}