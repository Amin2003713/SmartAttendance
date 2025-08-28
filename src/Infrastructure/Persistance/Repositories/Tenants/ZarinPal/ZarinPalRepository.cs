using Riviera.ZarinPal.V4;
using Riviera.ZarinPal.V4.Models;
using Shifty.Application.Base.ZarinPal.Request;
using Shifty.Application.Interfaces.ZarinPal;

namespace Shifty.Persistence.Repositories.Tenants.ZarinPal;

public class ZarinPalRepository(
    ZarinPalService _zarinpal
) : IZarinPal
{
    public async Task<ZarinPalResponse> CreatePaymentRequest(
        NewPayment paymentRequest,
        CancellationToken cancellationToken)
    {
        var result = await _zarinpal.RequestPaymentAsync(paymentRequest, cancellationToken);

        if (result.Error == null)
            return result!.Data.Adapt<ZarinPalResponse>();

        if (result.Error.Validations!.Count != 0)
            throw ShiftyException.Validation(result.Error.Message, result.Error.Validations);

        throw ShiftyException.BadRequest(result.Error.Message);
    }

    public async Task<ZarinPalVerifyResponse> VerifyPayment(
        ZarinPalVerifyRequest request,
        CancellationToken cancellationToken)
    {
        if (_zarinpal.IsStatusValid(request.Status) == false)
            throw ShiftyException.BadRequest("Invalid status");


        var result = await _zarinpal.VerifyPaymentAsync(request.Amount, request.Authority, cancellationToken);

        if (result.Error == null)
            if (result?.Data is { Code: 100 or 101 })
                return result.Data.Adapt<ZarinPalVerifyResponse>();

        if (result.Error.Validations!.Count != 0)
            throw ShiftyException.Validation(result.Error.Message, result.Error.Validations);


        throw ShiftyException.BadRequest("Invalid payment amount");
    }
}