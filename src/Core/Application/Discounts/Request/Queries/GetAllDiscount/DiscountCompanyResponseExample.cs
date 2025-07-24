using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Discounts.Request.Queries.GetAllDiscount;

public class DiscountCompanyResponseExample : IExamplesProvider<DiscountCompanyResponse>
{
    public DiscountCompanyResponse GetExamples()
    {
        return new DiscountCompanyResponse
        {
            CompanyName = "Example Co.", DateOfUse = DateTime.Now, IsUsed = true
        };
    }
}