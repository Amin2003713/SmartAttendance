using SmartAttendance.Application.Features.Vehicles.Requests.Commands.Create;

namespace SmartAttendance.Application.Features.Vehicles.Requests.Commands.Update;

public class UpdateVehicleRequest : CreateVehicleRequest
{
    public Guid Id { get; set; }
}