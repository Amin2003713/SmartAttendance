using Mapster;
using Microsoft.EntityFrameworkCore;
using Shifty.Application.Features.Stations.Queries.ById;
using Shifty.Application.Features.Stations.Requests.Queries.GetStations;
using Shifty.Application.Interfaces.Stations;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Features.Stations.Queries.GetById;

public class GetStationByIdQueryHandler(
    IStationQueryRepository queryRepository,
    ILogger<GetStationByIdQueryHandler> logger,
    IStringLocalizer<GetStationByIdQueryHandler> localizer)
    : IRequestHandler<GetStationByIdQuery, GetStationResponse>
{
    public async Task<GetStationResponse> Handle(GetStationByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Start handling GetStationById for StationId {StationId}", request.Id);

        var result = await queryRepository.TableNoTracking
            .Where(x => x.Id == request.Id)
            .ProjectToType<GetStationResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            logger.LogWarning("Station with Id {StationId} not found", request.Id);
            throw IpaException.NotFound(localizer["StationNotFound", request.Id]);
        }

        logger.LogInformation("Successfully retrieved Station {StationId}", request.Id);
        return result;
    }
}