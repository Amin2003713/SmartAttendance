using Mapster;
using SmartAttendance.Application.Features.Vehicles.Commands.Update;
using SmartAttendance.Application.Interfaces.Vehicles;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Vehicles;

namespace SmartAttendance.RequestHandlers.Features.Vehicles.Commands.Update;

public class UpdateVehicleCommandHandler(
    IVehicleQueryRepository queryRepository,
    IVehicleCommandRepository commandRepository,
    ILogger<UpdateVehicleCommandHandler> logger,
    IStringLocalizer<UpdateVehicleCommandHandler> localizer
) : IRequestHandler<UpdateVehicleCommand>
{
    public async Task Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await queryRepository.GetSingleAsync(cancellationToken, x => x.Id == request.Id);

            if (vehicle is null)
            {
                logger.LogWarning("Vehicles with ID {VehicleId} not found for update.", request.Id);
                throw SmartAttendanceException.NotFound(localizer["Vehicle not found."]);
            }

            vehicle.Update(request.Adapt<Vehicle>());

            await commandRepository.UpdateAsync(vehicle, cancellationToken);

            logger.LogInformation("Vehicles {VehicleId} updated successfully.", vehicle.Id);
        }

        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business error while updating vehicle {VehicleId}.", request.Id);
            throw;
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating vehicle {VehicleId}.", request.Id);
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while updating the vehicle."]);
        }
    }
}