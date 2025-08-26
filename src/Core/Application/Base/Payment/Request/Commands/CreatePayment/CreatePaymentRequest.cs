using Shifty.Common.General.Enums.Payment;

namespace Shifty.Application.Base.Payment.Request.Commands.CreatePayment;

public class CreatePaymentRequest
{
    public Guid? DiscountId { get; set; }
    public int UsersCount { get; set; }

    public PaymentType PaymentStatus { get; set; }
    public int? Duration { get; set; } = null;
}