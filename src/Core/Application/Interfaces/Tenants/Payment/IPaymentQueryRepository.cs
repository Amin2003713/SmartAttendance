using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Base.Payment.Request.Queries.ListPayment;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Tenants.Payments;

namespace SmartAttendance.Application.Interfaces.Tenants.Payment;

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