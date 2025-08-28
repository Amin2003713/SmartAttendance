using Mapster;
using Microsoft.EntityFrameworkCore;
using Shifty.Application.Features.Stations.Queries.ById;
using Shifty.Application.Features.Stations.Requests.Queries.GetStations;
using Shifty.Application.Interfaces.Stations;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Features.Stations.Queries.GetById;

public class GetStationByIdQueryHandler(
    IStationQueryRepository queryRepository)
    : IRequestHandler<GetStationByIdQuery, GetStationResponse>
{
    public async Task<GetStationResponse> Handle(GetStationByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await queryRepository.TableNoTracking
            .Where(x => x.Id == request.Id)
            .ProjectToType<GetStationResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        return result ?? new GetStationResponse();
    }
}