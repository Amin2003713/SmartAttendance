using Shifty.Application.Discounts.Queries.GetAllDiscount;
using Shifty.Application.Discounts.Request.Queries.GetAllDiscount;
using Shifty.Application.Interfaces.Tenants.Discounts;

namespace Shifty.RequestHandlers.Discounts.Queries.GetAllDiscount;

public class GetAllDiscountQueryHandler(
    IDiscountQueryRepository discountQueryRepository
)
    : IRequestHandler<GetAllDiscountQuery, List<GetAllDiscountResponse>>
{
    public async Task<List<GetAllDiscountResponse>> Handle(
        GetAllDiscountQuery request,
        CancellationToken cancellationToken)
    {
        return await discountQueryRepository.GetAllDiscounts(cancellationToken);
    }
}