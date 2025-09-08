using Mapster;
using SmartAttendance.Application.Features.Vehicles.Commands.Create;
using SmartAttendance.Application.Interfaces.Vehicles;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Vehicles;

namespace SmartAttendance.RequestHandlers.Features.Vehicles.Commands.Create;

public class CreateVehicleCommandHandler(
    ILogger<CreateVehicleCommandHandler> logger,
    IStringLocalizer<CreateVehicleCommandHandler> localizer,
    IVehicleCommandRepository commandRepository
) : IRequestHandler<CreateVehicleCommand>
{
    public async Task Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null)
                throw new InvalidNullInputException(nameof(request));

            logger.LogInformation("Creating Vehicle. Title={Title}, ",
                request.Title);

            var vehicle = request.Adapt<Vehicle>();

            await commandRepository.AddAsync(vehicle, cancellationToken);


            logger.LogInformation("Vehicle created. Id={VehicleId}, Name={Name}",
                vehicle.Id,
                vehicle.Title);
        }
        catch (SmartAttendanceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error when creating station. Name={Name}", request!.Title);
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while creating the station."]);
        }
    }
}