using Mapster;
using SmartAttendance.Application.Base.Payment.Queries.GetActivePayment;
using SmartAttendance.Application.Base.Payment.Request.Queries.ListPayment;
using SmartAttendance.Application.Interfaces.Tenants.Payment;

namespace SmartAttendance.RequestHandlers.Base.Payment.Queries.GetActivePayment;

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