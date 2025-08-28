using Mapster;
using Shifty.Application.Features.Vehicles.Commands.Create;
using Shifty.Application.Interfaces.Vehicles;
using Shifty.Common.Exceptions;
using Shifty.Domain.Vehicles;

namespace Shifty.RequestHandlers.Features.Vehicles.Commands.Create;

public class CreateVehicleCommandHandler(
    ILogger<CreateVehicleCommandHandler> logger,
    IStringLocalizer<CreateVehicleCommandHandler> localizer,
    IVehicleCommandRepository commandRepository) : IRequestHandler<CreateVehicleCommand>
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


            logger.LogInformation("Vehicle created. Id={VehicleId}, Name={Name}", vehicle.Id,
                vehicle.Title);
        }
        catch (ShiftyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error when creating station. Name={Name}", request!.Title);
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while creating the station."]);
        }
    }
}