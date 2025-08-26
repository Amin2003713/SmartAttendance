using Shifty.Application.Base.Payment.Commands.AddActiveUser;
using Shifty.Application.Interfaces.Tenants.Payment;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Base.Payment.Commands.AddActiveUser;

public class AddActiveUserCommandHandler(
    IPaymentQueryRepository queryRepository,
    IPaymentCommandRepository paymentCommandRepository,
    ILogger<AddActiveUserCommandHandler> logger,
    IStringLocalizer<AddActiveUserCommandHandler> localizer
)
    : IRequestHandler<AddActiveUserCommand, int>
{
    public async Task<int> Handle(AddActiveUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var companyPurchase = await queryRepository.GetPayment(cancellationToken);

            if (companyPurchase == null)
            {
                logger.LogWarning("No active company purchase/payment record found.");
                throw IpaException.NotFound(localizer["No active payment record found for this tenant."]);
            }

            companyPurchase.ActiveUsers++;

            await paymentCommandRepository.Update(companyPurchase, cancellationToken);

            logger.LogInformation("Active users incremented to {ActiveUsers} for payment record {PaymentId}.",
                companyPurchase.ActiveUsers,
                companyPurchase.Id);

            return companyPurchase.ActiveUsers;
        }
        catch (IpaException ex)
        {
            logger.LogError(ex, "Business error occurred while adding active user.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while adding active user.");
            throw IpaException.InternalServerError(
                localizer["An unexpected error occurred while adding an active user."]);
        }
    }
}