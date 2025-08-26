using Shifty.Common.General.Enums.Discount;

namespace Shifty.Application.Base.Discounts.Request.Commands.CreateDisCount;

public class CreateDiscountRequestExample : IExamplesProvider<CreateDiscountRequest>
{
    public CreateDiscountRequest GetExamples()
    {
        return new CreateDiscountRequest
        {
            Code = "SPRING2025",
            StartDate = DateTime.UtcNow,
            Duration = 30,
            DiscountType = DiscountType.Percent, // or FixedAmount depending on your enum
            Value = 15.5m,
            PackageMonth = 6,
            TenantIds = new List<string>() // ← as requested, leave this empty
        };
    }
}