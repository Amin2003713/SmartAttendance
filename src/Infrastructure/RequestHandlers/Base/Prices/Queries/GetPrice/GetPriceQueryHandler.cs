using Mapster;
using SmartAttendance.Application.Base.Prices.Queries.GetPrice;
using SmartAttendance.Application.Base.Prices.Request.Queries.GetPrice;
using SmartAttendance.Application.Interfaces.Tenants.Prices;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Base.Prices.Queries.GetPrice;

public class GetPriceQueryHandler(
    IPriceQueryRepository priceQueryRepository,
    ILogger<GetPriceQueryHandler> logger,
    IStringLocalizer<GetPriceQueryHandler> localizer
) : IRequestHandler<GetPriceQuery, GetPriceResponse>
{
    public async Task<GetPriceResponse> Handle(GetPriceQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var price = await priceQueryRepository.GetPrice(cancellationToken);

            if (price == null)
            {
                logger.LogWarning("No active price found.");
                throw SmartAttendanceException.NotFound(localizer["No price configuration found."]);
            }

            logger.LogInformation("Price retrieved successfully. ID: {Id}", price.Id);

            return price.Adapt<GetPriceResponse>();
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business exception occurred while retrieving price.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while retrieving price.");
            throw SmartAttendanceException.InternalServerError(localizer["An unexpected error occurred while retrieving price."]);
        }
    }
}