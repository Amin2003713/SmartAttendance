using Shifty.Common.General.Enums.Discount;

namespace Shifty.Application.Discounts.Request.Queries.CheckDiscount;

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