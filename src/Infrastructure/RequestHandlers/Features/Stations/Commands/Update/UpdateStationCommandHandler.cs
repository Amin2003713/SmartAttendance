using Mapster;
using Shifty.Application.Features.Stations.Commands.Update;
using Shifty.Application.Interfaces.Stations;
using Shifty.Common.Exceptions;
using Shifty.Domain.Stations;

namespace Shifty.RequestHandlers.Features.Stations.Commands.Update;

public class UpdateStationCommandHandler(
    IStationCommandRepository commandRepository,
    IStationQueryRepository queryRepository,
    ILogger<UpdateStationCommandHandler> logger,
    IStringLocalizer<UpdateStationCommandHandler> localizer) : IRequestHandler<UpdateStationCommand>
{
    public async Task Handle(UpdateStationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var station = await queryRepository.GetSingleAsync(cancellationToken, x => x.Id == request.Id);
            if (station is null)
            {
                logger.LogWarning("Stations with ID {StationId} not found for update.", request.Id);
                throw IpaException.NotFound(localizer["Station not found."]);
            }

            station.Update(request.Adapt<Station>());

            await commandRepository.UpdateAsync(station, cancellationToken);

            logger.LogInformation("Stations {StationId} updated successfully.", station.Id);
        }


        catch (IpaException ex)
        {
            logger.LogError(ex, "Business error while updating station {StationId}.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating station {StationId}.", request.Id);
            throw IpaException.InternalServerError(
                localizer["An unexpected error occurred while updating the station."]);
        }
    }
}