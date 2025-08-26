using Mapster;
using Microsoft.EntityFrameworkCore;
using Shifty.Application.Features.Stations.Queries.ById;
using Shifty.Application.Features.Stations.Requests.Queries.GetStations;
using Shifty.Application.Interfaces.Stations;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Features.Stations.Queries.ById;

public  class GetStationByIdQueryHandler(
    IStationQueryRepository departmentQueryRepository
) : IRequestHandler< GetStationByIdQuery,  GetStationResponse>
{
    public async Task<GetStationResponse> Handle(GetStationByIdQuery request, CancellationToken cancellationToken)
    {
        if (! await departmentQueryRepository.AnyAsync(department => department.Id == request.Id  , cancellationToken))
            throw IpaException.NotFound("department not found");

        return (await departmentQueryRepository.TableNoTracking.Where(a => a.Id == request.Id).SingleOrDefaultAsync(cancellationToken: cancellationToken))
            .Adapt<GetStationResponse>();
    }
}