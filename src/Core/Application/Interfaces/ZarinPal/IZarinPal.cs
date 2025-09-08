using System.Threading;
using System.Threading.Tasks;
using Riviera.ZarinPal.V4.Models;
using SmartAttendance.Application.Base.ZarinPal.Request;
using SmartAttendance.Common.Utilities.InjectionHelpers;

namespace SmartAttendance.Application.Interfaces.ZarinPal;

public interface IZarinPal : IScopedDependency
{
    public Task<ZarinPalResponse> CreatePaymentRequest(
        NewPayment paymentRequest,
        CancellationToken cancellationToken);

    Task<ZarinPalVerifyResponse> VerifyPayment(ZarinPalVerifyRequest request, CancellationToken cancellationToken);
}