using Mapster;
using SmartAttendance.Application.Features.Stations.Commands.Update;
using SmartAttendance.Application.Interfaces.Stations;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Stations;

namespace SmartAttendance.RequestHandlers.Features.Stations.Commands.Update;

public class UpdateStationCommandHandler(
    IStationCommandRepository commandRepository,
    IStationQueryRepository queryRepository,
    ILogger<UpdateStationCommandHandler> logger,
    IStringLocalizer<UpdateStationCommandHandler> localizer
) : IRequestHandler<UpdateStationCommand>
{
    public async Task Handle(UpdateStationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var station = await queryRepository.GetSingleAsync(cancellationToken, x => x.Id == request.Id);

            if (station is null)
            {
                logger.LogWarning("Stations with ID {StationId} not found for update.", request.Id);
                throw SmartAttendanceException.NotFound(localizer["Station not found."]);
            }

            station.Update(request.Adapt<Station>());

            await commandRepository.UpdateAsync(station, cancellationToken);

            logger.LogInformation("Stations {StationId} updated successfully.", station.Id);
        }


        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business error while updating station {StationId}.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating station {StationId}.", request.Id);
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while updating the station."]);
        }
    }
}