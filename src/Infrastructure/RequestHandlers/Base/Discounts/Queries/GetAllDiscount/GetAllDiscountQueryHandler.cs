using Shifty.Application.Base.Discounts.Queries.GetAllDiscount;
using Shifty.Application.Base.Discounts.Request.Queries.GetAllDiscount;
using Shifty.Application.Interfaces.Tenants.Discounts;

namespace Shifty.RequestHandlers.Base.Discounts.Queries.GetAllDiscount;

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