using SmartAttendance.Application.Base.Discounts.Queries.GetAllDiscount;
using SmartAttendance.Application.Base.Discounts.Request.Queries.GetAllDiscount;
using SmartAttendance.Application.Interfaces.Tenants.Discounts;

namespace SmartAttendance.RequestHandlers.Base.Discounts.Queries.GetAllDiscount;

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