using Shifty.Application.Features.Vehicles.Requests.Queries.GetVehicles;
using Shifty.Common.Common.Requests;

namespace Shifty.Application.Features.Vehicles.Queries.GetVehicles;

public class GetVehiclesQuery : IRequest<List<GetVehicleQueryResponse>>;