using DNTPersianUtils.Core;
using Shifty.Common.General.Enums.Discount;

namespace Shifty.Application.Base.Discounts.Request.Queries.GetAllDiscount;

public class GetAllDiscountResponseExample : IExamplesProvider<GetAllDiscountResponse>
{
    public GetAllDiscountResponse GetExamples()
    {
        return new GetAllDiscountResponse
        {
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            Id = Guid.NewGuid(),
            Code = "SUMMER2025",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            Duration = 30,
            IsActive = true,
            RemainingDays = new TimeSpanParts(DateTime.UtcNow.AddDays(30) - DateTime.UtcNow),
            DiscountCompanyUsage =
            [
                new DiscountCompanyResponse
                {
                    CompanyName = "Example Co.",
                    DateOfUse = DateTime.UtcNow,
                    IsUsed = true
                }
            ],
            DiscountType = DiscountType.FixedAmount,
            Value = 20000,
            PackageMonth = 12
        };
    }
}