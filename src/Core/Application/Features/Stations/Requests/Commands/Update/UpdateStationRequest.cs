using Shifty.Application.Features.Stations.Requests.Commands.Create;

namespace Shifty.Application.Features.Stations.Requests.Commands.Update;

public class UpdateStationRequest : CreateStationRequest
{
    public Guid Id { get; set; }
}