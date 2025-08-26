using Shifty.Application.Base.Payment.Commands.CreatePayment;
using Shifty.Application.Interfaces.Tenants.Payment;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Base.Payment.Commands.CreatePayment;

public class CreatePaymentCommandHandler(
    IPaymentCommandRepository paymentCommandRepository,
    ILogger<CreatePaymentCommandHandler> logger,
    IStringLocalizer<CreatePaymentCommandHandler> localizer
)
    : IRequestHandler<CreatePaymentCommand, string>
{
    public async Task<string> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var paymentUri = await paymentCommandRepository.CreatePayment(request, cancellationToken);

            if (paymentUri == null)
            {
                logger.LogWarning("Payment creation failed for tenant.");
                throw IpaException.BadRequest(localizer["Unable to create payment."]);
            }

            logger.LogInformation("Payment created successfully: {PaymentUri}", paymentUri.AbsoluteUri);

            return paymentUri.AbsoluteUri;
        }
        catch (IpaException ex)
        {
            logger.LogError(ex, "Business error occurred while creating payment.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while creating payment.");
            throw IpaException.InternalServerError(localizer["An unexpected error occurred while creating payment."]);
        }
    }
}