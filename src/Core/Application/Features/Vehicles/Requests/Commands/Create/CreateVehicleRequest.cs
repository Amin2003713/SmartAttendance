using Shifty.Common.Common.Requests;

namespace Shifty.Application.Features.Vehicles.Requests.Commands.Create;

public class CreateVehicleRequest
{
    public string Title { get; set; }
    public PlateNumber PlateNumber { get; set; }
}