using Shifty.Common.Common.Requests;

namespace Shifty.Application.Features.Vehicles.Requests.Queries.GetVehicles;

public class GetVehicleQueryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public PlateNumber PlateNumber { get; set; }
}