using Shifty.Common.Common.Requests;
using Shifty.Common.General.Enums;

namespace Shifty.Application.Features.Vehicles.Requests.Commands.Create;

public class CreateVehicleRequest
{
    public required string Title { get; set; }

    public required PlateNumber PlateNumber { get; set; }

    public VehicleType VehicleType { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public DateTime YearOfProduction { get; set; }
    public Guid ResponsibleId { get; set; }
    public string Descriprion { get; set; }
    public VehicleStatus Status { get; set; }
}