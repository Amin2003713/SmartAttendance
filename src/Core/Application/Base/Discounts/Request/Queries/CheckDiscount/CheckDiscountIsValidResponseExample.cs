using SmartAttendance.Common.General.Enums.Discount;

namespace SmartAttendance.Application.Base.Discounts.Request.Queries.CheckDiscount;

public class CheckDiscountIsValidResponseExample : IExamplesProvider<CheckDiscountIsValidResponse>
{
    public CheckDiscountIsValidResponse GetExamples()
    {
        return new CheckDiscountIsValidResponse
        {
            DiscountType = DiscountType.Percent,
            Value = 10.0m,
            Id = Guid.NewGuid()
        };
    }
}