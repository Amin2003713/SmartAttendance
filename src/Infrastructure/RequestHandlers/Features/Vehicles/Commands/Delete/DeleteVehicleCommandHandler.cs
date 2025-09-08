using SmartAttendance.Application.Features.Vehicles.Commands.Delete;
using SmartAttendance.Application.Interfaces.Vehicles;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Vehicles.Commands.Delete;

public class DeleteVehicleCommandHandler(
    IVehicleQueryRepository queryRepository,
    IVehicleCommandRepository commandRepository,
    ILogger<DeleteVehicleCommandHandler> logger,
    IStringLocalizer<DeleteVehicleCommandHandler> localizer
) : IRequestHandler<DeleteVehicleCommand>
{
    public async Task Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await queryRepository.GetSingleAsync(cancellationToken, x => x.Id == request.Id);

            if (vehicle is null)
            {
                logger.LogWarning("Vehicles with ID {VehicleId} not found for delete.", request.Id);
                throw SmartAttendanceException.NotFound(localizer["Vehicle not found."]);
            }

            await commandRepository.DeleteAsync(vehicle, cancellationToken);

            logger.LogInformation("Vehicle with ID {VehicleId} deleted successfully.", request.Id);
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business error while deleting vehicle {VehicleId}.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while deleting vehicle {VehicleId}.", request.Id);
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while deleting the vehicle."]);
        }
    }
}