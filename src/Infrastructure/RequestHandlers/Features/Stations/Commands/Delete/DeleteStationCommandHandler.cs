using Shifty.Application.Features.Stations.Commands.Delete;
using Shifty.Application.Interfaces.Stations;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Features.Stations.Commands.Delete;

public class DeleteStationCommandHandler(
    IStationCommandRepository commandRepository,
    IStationQueryRepository queryRepository,
    ILogger<DeleteStationCommandHandler> logger,
    IStringLocalizer<DeleteStationCommandHandler> localizer) : IRequestHandler<DeleteStationCommand>
{
    public async Task Handle(DeleteStationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var station = await queryRepository.FirstOrDefaultsAsync(x => x.Id == request.Id, cancellationToken);
            if (station is null)
            {
                logger.LogWarning("Stations with ID {StationId} not found for delete.", request.Id);
                throw ShiftyException.NotFound(localizer["Station not found."]);
            }

            await commandRepository.DeleteAsync(station, cancellationToken);

            logger.LogInformation("Station with ID {StationId} deleted successfully.", request.Id);
        }
        catch (ShiftyException ex)
        {
            logger.LogError(ex, "Business error while deleting station {StationId}.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while deleting station {StationId}.", request.Id);
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while deleting the station."]);
        }
    }
}