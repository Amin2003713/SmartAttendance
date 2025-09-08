using SmartAttendance.Application.Features.Stations.Requests.Queries.GetStations;

namespace SmartAttendance.Application.Features.Stations.Queries.ById;

public record GetStationByIdQuery(
    Guid Id
) : IRequest<GetStationResponse>;