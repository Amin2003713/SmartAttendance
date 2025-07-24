using Mapster;
using Shifty.Application.Interfaces.Tenants.Payment;
using Shifty.Application.Payment.Queries.GetActivePayment;
using Shifty.Application.Payment.Request.Queries.ListPayment;

namespace Shifty.RequestHandlers.Payment.Queries.GetActivePayment;

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