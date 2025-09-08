using SmartAttendance.Application.Features.Missions.Requests.Commands.Create;

namespace SmartAttendance.Application.Features.Missions.Requests.Commands.Update;

public class UpdateMissionRequest :  CreateMissionRequest
{
    public Guid AggregateId { set; get; }
}