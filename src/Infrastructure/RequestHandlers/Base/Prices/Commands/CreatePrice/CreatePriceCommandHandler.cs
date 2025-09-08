using Mapster;
using SmartAttendance.Application.Base.Prices.Commands.CreatePrice;
using SmartAttendance.Application.Interfaces.Tenants.Prices;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Tenants;

namespace SmartAttendance.RequestHandlers.Base.Prices.Commands.CreatePrice;

public class CreatePriceCommandHandler(
    IPriceCommandRepository commandRepository,
    ILogger<CreatePriceCommandHandler> logger,
    IStringLocalizer<CreatePriceCommandHandler> localizer
) : IRequestHandler<CreatePriceCommand>
{
    public async Task Handle(CreatePriceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null)
                throw new InvalidNullInputException(nameof(request));

            var entity = request.Adapt<Price>();
            await commandRepository.CreateNewPrice(entity, cancellationToken);

            logger.LogInformation("Price created successfully: {Title}", entity.Amount);
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business exception occurred while creating price.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while creating price.");
            throw SmartAttendanceException.InternalServerError(localizer["An unexpected error occurred while creating the price."]);
        }
    }
}