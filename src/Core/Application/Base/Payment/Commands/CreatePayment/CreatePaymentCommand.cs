using SmartAttendance.Application.Base.Payment.Request.Commands.CreatePayment;

namespace SmartAttendance.Application.Base.Payment.Commands.CreatePayment;

public class CreatePaymentCommand : CreatePaymentRequest,
    IRequest<string>;