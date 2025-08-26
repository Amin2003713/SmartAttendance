using Shifty.Application.Features.Vehicles.Requests.Commands.Create;

namespace Shifty.Application.Features.Vehicles.Requests.Commands.Update;

public class UpdateVehicleRequest : CreateVehicleRequest
{
    public Guid Id { get; set; }
}