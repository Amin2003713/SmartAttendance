using Mapster;
using Microsoft.EntityFrameworkCore;
using Shifty.Application.Features.Stations.Queries.GetStations;
using Shifty.Application.Features.Stations.Requests.Queries.GetStations;
using Shifty.Application.Interfaces.Stations;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Features.Stations.Queries.GetStations;

public class GetStationsQueryHandler(
    IStationQueryRepository queryRepository,
    ILogger<GetStationsQueryHandler> logger) : IRequestHandler<GetStationsQuery, List<GetStationResponse>>
{
    public async Task<List<GetStationResponse>> Handle(GetStationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var response = await queryRepository.TableNoTracking.Where(x => x.DeletedBy == null)
                .ProjectToType<GetStationResponse>()
                .ToListAsync(cancellationToken);

            if (response is null)
                return [];


            logger.LogInformation("{Count} Station found ", response.Count);

            return response;
        }
        catch (ShiftyException ex)
        {
            logger.LogError(ex, "Business exception occurred while retrieving stations.");
            throw;
        }
    }
}