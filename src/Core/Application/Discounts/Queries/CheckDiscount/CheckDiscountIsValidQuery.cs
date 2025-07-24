using Shifty.Application.Discounts.Request.Queries.CheckDiscount;

namespace Shifty.Application.Discounts.Queries.CheckDiscount;

public class CheckDiscountIsValidQuery : IRequest<CheckDiscountIsValidResponse>
{
    public int PackageMonth { get; set; }
    public string Code { get; set; }

    public static CheckDiscountIsValidQuery Create(string code, int packageMonth)
    {
        return new CheckDiscountIsValidQuery
        {
            Code = code, PackageMonth = packageMonth
        };
    }
}