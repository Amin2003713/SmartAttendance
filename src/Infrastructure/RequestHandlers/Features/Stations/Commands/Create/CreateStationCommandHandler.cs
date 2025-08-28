using Mapster;
using Shifty.Application.Features.Stations.Commands.Create;
using Shifty.Application.Interfaces.Stations;
using Shifty.Common.Exceptions;
using Shifty.Domain.Stations;

namespace Shifty.RequestHandlers.Features.Stations.Commands.Create;

public class CreateStationCommandHandler(
    IStationCommandRepository commandRepository,
    ILogger<CreateStationCommandHandler> logger,
    IStringLocalizer<CreateStationCommandHandler> localizer) : IRequestHandler<CreateStationCommand>
{
    public async Task Handle(CreateStationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            logger.LogInformation("Creating Station. Name={Name}, ",
                request.Name);

            var station = request.Adapt<Station>();

            await commandRepository.AddAsync(station, cancellationToken);


            logger.LogInformation("Station created. Id={StationId}, Name={Name}", station.Id,
                station.Name);
        }
        catch (ShiftyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error when creating station. Name={Name}", request!.Name);
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while creating the station."]);
        }
    }
}