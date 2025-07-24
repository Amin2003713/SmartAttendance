using Shifty.Application.Payment.Request.Queries.ListPayment;

namespace Shifty.Application.Payment.Queries.GetActivePayment;

public record GetActivePaymentQuery : IRequest<PaymentQueryResponse>;