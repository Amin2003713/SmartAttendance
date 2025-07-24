using DNTPersianUtils.Core;
using Shifty.Common.General.Enums.Discount;
using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Discounts.Request.Queries.GetAllDiscount;

public class GetAllDiscountResponseExample : IExamplesProvider<GetAllDiscountResponse>
{
    public GetAllDiscountResponse GetExamples()
    {
        return new GetAllDiscountResponse
        {
            CreatedAt = DateTime.Now.AddDays(-10),
            Id = Guid.NewGuid(),
            Code = "SUMMER2025",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30),
            Duration = 30,
            IsActive = true,
            RemainingDays = new TimeSpanParts(DateTime.Now.AddDays(30) - DateTime.Now),
            DiscountCompanyUsage =
            [
                new DiscountCompanyResponse
                {
                    CompanyName = "Example Co.", DateOfUse = DateTime.Now, IsUsed = true
                }
            ],
            DiscountType = DiscountType.FixedAmount,
            Value = 20000,
            PackageMonth = 12
        };
    }
}