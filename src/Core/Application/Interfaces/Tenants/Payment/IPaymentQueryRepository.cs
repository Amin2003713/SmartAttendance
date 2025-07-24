using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Payment.Request.Queries.ListPayment;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Tenants.Payments;

namespace Shifty.Application.Interfaces.Tenants.Payment;

public interface IPaymentQueryRepository : IScopedDependency
{
    Task<List<PaymentQueryResponse>> ListPayments(CancellationToken cancellationToken);
    Task<Payments?>                  GetPayment(CancellationToken cancellationToken);
    Task<Payments>                   GetPayment(string authority, CancellationToken cancellationToken);
    Task<Payments>                   GetPayment(Guid paymentId, CancellationToken cancellationToken);

    Task<Payments> GetPaymentWithSuccess(
        Guid paymentId,
        CancellationToken cancellationToken = default);
}