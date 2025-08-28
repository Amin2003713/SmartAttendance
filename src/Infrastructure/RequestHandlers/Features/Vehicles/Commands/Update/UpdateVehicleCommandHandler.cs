using Mapster;
using Shifty.Application.Features.Vehicles.Commands.Update;
using Shifty.Application.Interfaces.Vehicles;
using Shifty.Common.Exceptions;
using Shifty.Domain.Vehicles;

namespace Shifty.RequestHandlers.Features.Vehicles.Commands.Update;

public class UpdateVehicleCommandHandler(
    IVehicleQueryRepository queryRepository,
    IVehicleCommandRepository commandRepository,
    ILogger<UpdateVehicleCommandHandler> logger,
    IStringLocalizer<UpdateVehicleCommandHandler> localizer) : IRequestHandler<UpdateVehicleCommand>
{
    public async Task Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await queryRepository.GetSingleAsync(cancellationToken, x => x.Id == request.Id);

            if (vehicle is null)
            {
                logger.LogWarning("Vehicles with ID {VehicleId} not found for update.", request.Id);
                throw ShiftyException.NotFound(localizer["Vehicle not found."]);
            }

            vehicle.Update(request.Adapt<Vehicle>());

            await commandRepository.UpdateAsync(vehicle, cancellationToken);

            logger.LogInformation("Vehicles {VehicleId} updated successfully.", vehicle.Id);
        }

        catch (ShiftyException ex)
        {
            logger.LogError(ex, "Business error while updating vehicle {VehicleId}.", request.Id);
            throw;
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating vehicle {VehicleId}.", request.Id);
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while updating the vehicle."]);
        }
    }
}