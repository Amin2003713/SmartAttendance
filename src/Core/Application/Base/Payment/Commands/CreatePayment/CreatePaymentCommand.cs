using Shifty.Application.Base.Payment.Request.Commands.CreatePayment;

namespace Shifty.Application.Base.Payment.Commands.CreatePayment;

public class CreatePaymentCommand : CreatePaymentRequest,
    IRequest<string>;