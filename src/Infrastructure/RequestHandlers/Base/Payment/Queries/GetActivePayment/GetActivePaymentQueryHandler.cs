using Mapster;
using Shifty.Application.Base.Payment.Queries.GetActivePayment;
using Shifty.Application.Base.Payment.Request.Queries.ListPayment;
using Shifty.Application.Interfaces.Tenants.Payment;

namespace Shifty.RequestHandlers.Base.Payment.Queries.GetActivePayment;

public class GetActivePaymentQueryHandler(
    IPaymentQueryRepository paymentQueryRepository
)
    : IRequestHandler<GetActivePaymentQuery, PaymentQueryResponse>
{
    public async Task<PaymentQueryResponse> Handle(GetActivePaymentQuery request, CancellationToken cancellationToken)
    {
        return (await paymentQueryRepository.GetPayment(cancellationToken)).Adapt<PaymentQueryResponse>();
    }
}