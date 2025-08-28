using Shifty.Application.Features.Vehicles.Requests.Queries.GetVehicles;

namespace Shifty.Application.Features.Vehicles.Queries.GetById;

public record GetVehicleByIdQuery(Guid Id) : IRequest<GetVehicleQueryResponse>;