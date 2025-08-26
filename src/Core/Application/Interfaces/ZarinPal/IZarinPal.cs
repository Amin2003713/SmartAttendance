using System.Threading;
using System.Threading.Tasks;
using Riviera.ZarinPal.V4.Models;
using Shifty.Application.Base.ZarinPal.Request;
using Shifty.Common.Utilities.InjectionHelpers;

namespace Shifty.Application.Interfaces.ZarinPal;

public interface IZarinPal : IScopedDependency
{
    public Task<ZarinPalResponse> CreatePaymentRequest(
        NewPayment paymentRequest,
        CancellationToken cancellationToken);

    Task<ZarinPalVerifyResponse> VerifyPayment(ZarinPalVerifyRequest request, CancellationToken cancellationToken);
}