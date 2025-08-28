using Mapster;
using Microsoft.EntityFrameworkCore;
using Shifty.Application.Features.Vehicles.Queries.GetById;
using Shifty.Application.Features.Vehicles.Requests.Queries.GetVehicles;
using Shifty.Application.Interfaces.Vehicles;

namespace Shifty.RequestHandlers.Features.Vehicles.Queries.GetById;

public class GetVehicleByIdQueryHandler(
    IVehicleQueryRepository queryRepository
)
    : IRequestHandler<GetVehicleByIdQuery, GetVehicleQueryResponse>
{
    public async Task<GetVehicleQueryResponse> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await queryRepository.TableNoTracking.Where(x => x.Id == request.Id)
            .ProjectToType<GetVehicleQueryResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        return vehicle ?? new GetVehicleQueryResponse();
    }
}