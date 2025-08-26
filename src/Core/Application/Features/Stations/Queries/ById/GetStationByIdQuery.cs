using Shifty.Application.Features.Stations.Requests.Queries.GetStations;

namespace Shifty.Application.Features.Stations.Queries.ById;

public record GetStationByIdQuery(Guid Id) : IRequest<GetStationResponse>;