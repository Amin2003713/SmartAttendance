using SmartAttendance.Application.Base.Payment.Queries.ListPayments;
using SmartAttendance.Application.Base.Payment.Request.Queries.ListPayment;
using SmartAttendance.Application.Interfaces.Tenants.Payment;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Base.Payment.Queries.ListPayment;

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
        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business exception occurred while listing payments.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while listing payments.");
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while retrieving the payments."]);
        }
    }
}