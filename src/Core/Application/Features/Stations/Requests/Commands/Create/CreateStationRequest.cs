using Shifty.Common.Common.Requests;
using Shifty.Common.General.Enums.StationStatuses;
using Shifty.Common.General.Enums.StationTypes;

namespace Shifty.Application.Features.Stations.Requests.Commands.Create;

public class CreateStationRequest
{
    public string Name { get; set; }

    public string Code { get; set; }

    public decimal AllowedDistance { get; set; }

    public TimeSpan OnWay { get; set; }

    public Location Location { get; set; }

    public StationStatus StationStatus { get; set; }

    public StationType StationType { get; set; }
}