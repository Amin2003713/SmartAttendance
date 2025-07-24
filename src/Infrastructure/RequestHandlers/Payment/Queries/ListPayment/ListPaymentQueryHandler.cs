using Shifty.Application.Interfaces.Tenants.Payment;
using Shifty.Application.Payment.Queries.ListPayments;
using Shifty.Application.Payment.Request.Queries.ListPayment;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Payment.Queries.ListPayment;

public class ListPaymentQueryHandler(
    IPaymentQueryRepository paymentQueryRepository,
    ILogger<ListPaymentQueryHandler> logger,
    IStringLocalizer<ListPaymentQueryHandler> localizer
)
    : IRequestHandler<ListPaymentsQuery, List<PaymentQueryResponse>>
{
    public async Task<List<PaymentQueryResponse>> Handle(ListPaymentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var result = await paymentQueryRepository.ListPayments(cancellationToken);

            logger.LogInformation("Retrieved {Count} payments.", result.Count);
            return result;
        }
        catch (IpaException ex)
        {
            logger.LogError(ex, "Business exception occurred while listing payments.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while listing payments.");
            throw IpaException.InternalServerError(
                localizer["An unexpected error occurred while retrieving the payments."]);
        }
    }
}