using SmartAttendance.Application.Base.Payment.Commands.CreatePayment;
using SmartAttendance.Application.Interfaces.Tenants.Payment;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Base.Payment.Commands.CreatePayment;

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
                throw SmartAttendanceException.BadRequest(localizer["Unable to create payment."]);
            }

            logger.LogInformation("Payment created successfully: {PaymentUri}", paymentUri.AbsoluteUri);

            return paymentUri.AbsoluteUri;
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business error occurred while creating payment.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while creating payment.");
            throw SmartAttendanceException.InternalServerError(localizer["An unexpected error occurred while creating payment."]);
        }
    }
}