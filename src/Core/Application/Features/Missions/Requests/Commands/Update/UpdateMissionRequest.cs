using Shifty.Application.Features.Missions.Requests.Commands.Create;

namespace Shifty.Application.Features.Missions.Requests.Commands.Update;

public class UpdateMissionRequest :  CreateMissionRequest
{
    public Guid AggregateId { set; get; }
}