using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Payment.Commands.CreatePayment;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Tenants.Payments;

namespace Shifty.Application.Interfaces.Tenants.Payment;

public interface IPaymentCommandRepository : IScopedDependency
{
    Task<Uri?> CreatePayment(CreatePaymentCommand createPayment, CancellationToken cancellationToken);
    Task       Update(Payments payments, CancellationToken cancellationToken);
}