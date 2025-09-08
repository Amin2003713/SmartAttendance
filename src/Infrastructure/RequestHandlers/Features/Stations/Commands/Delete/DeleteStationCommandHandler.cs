using SmartAttendance.Application.Features.Stations.Commands.Delete;
using SmartAttendance.Application.Interfaces.Stations;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Stations.Commands.Delete;

public class DeleteStationCommandHandler(
    IStationCommandRepository commandRepository,
    IStationQueryRepository queryRepository,
    ILogger<DeleteStationCommandHandler> logger,
    IStringLocalizer<DeleteStationCommandHandler> localizer
) : IRequestHandler<DeleteStationCommand>
{
    public async Task Handle(DeleteStationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var station = await queryRepository.FirstOrDefaultsAsync(x => x.Id == request.Id, cancellationToken);

            if (station is null)
            {
                logger.LogWarning("Stations with ID {StationId} not found for delete.", request.Id);
                throw SmartAttendanceException.NotFound(localizer["Station not found."]);
            }

            await commandRepository.DeleteAsync(station, cancellationToken);

            logger.LogInformation("Station with ID {StationId} deleted successfully.", request.Id);
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business error while deleting station {StationId}.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while deleting station {StationId}.", request.Id);
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while deleting the station."]);
        }
    }
}