using Shifty.Application.Stations.Requests.Commands.Create;
using Shifty.Common.Common.Requests;
using Shifty.Common.General.Enums.Stationstatuses;
using Shifty.Common.General.Enums.StationTypes;

namespace Shifty.Application.Stations.Requests.Commands.Update;

public class UpdateStationRequest : CreateStationRequest
{
    public Guid Id { get; set; }
}