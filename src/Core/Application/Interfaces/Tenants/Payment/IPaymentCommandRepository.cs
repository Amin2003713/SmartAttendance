using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Base.Payment.Commands.CreatePayment;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Tenants.Payments;

namespace SmartAttendance.Application.Interfaces.Tenants.Payment;

public interface IPaymentCommandRepository : IScopedDependency
{
    Task<Uri?> CreatePayment(CreatePaymentCommand createPayment, CancellationToken cancellationToken);
    Task       Update(Payments payments, CancellationToken cancellationToken);
}