using Shifty.Application.Base.Payment.Request.Queries.ListPayment;

namespace Shifty.Application.Base.Payment.Queries.GetActivePayment;

public record GetActivePaymentQuery : IRequest<PaymentQueryResponse>;