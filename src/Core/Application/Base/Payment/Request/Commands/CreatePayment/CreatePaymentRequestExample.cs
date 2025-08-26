using Shifty.Common.General.Enums.Payment;

namespace Shifty.Application.Base.Payment.Request.Commands.CreatePayment;

public class CreatePaymentRequestExample : IExamplesProvider<CreatePaymentRequest>
{
    public CreatePaymentRequest GetExamples()
    {
        return new CreatePaymentRequest
        {
            DiscountId = Guid.Empty,
            UsersCount = 5,
            PaymentStatus = PaymentType.RenewSubscription,
            Duration = 12
        };
    }
}