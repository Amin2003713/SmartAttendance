using Shifty.Application.Payment.Request.Commands.CreatePayment;

namespace Shifty.Application.Payment.Commands.CreatePayment;

public class CreatePaymentCommand : CreatePaymentRequest,
    IRequest<string>;