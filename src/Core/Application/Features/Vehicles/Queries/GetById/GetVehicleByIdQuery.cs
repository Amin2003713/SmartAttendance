using SmartAttendance.Application.Features.Vehicles.Requests.Queries.GetVehicles;

namespace SmartAttendance.Application.Features.Vehicles.Queries.GetById;

public record GetVehicleByIdQuery(
    Guid Id
) : IRequest<GetVehicleQueryResponse>;