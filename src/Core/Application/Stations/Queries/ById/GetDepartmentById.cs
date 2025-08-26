using Shifty.Application.Departments.Requests.Queries.GetDepartments;
using Shifty.Application.Stations.Requests.Queries.GetStations;

namespace Shifty.Application.Stations.Queries.ById;

public record GetStationByIdQuery(Guid Id) : IRequest<GetStationResponse>;