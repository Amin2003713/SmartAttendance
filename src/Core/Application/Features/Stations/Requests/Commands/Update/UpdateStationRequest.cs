using SmartAttendance.Application.Features.Stations.Requests.Commands.Create;

namespace SmartAttendance.Application.Features.Stations.Requests.Commands.Update;

public class UpdateStationRequest : CreateStationRequest
{
    public Guid Id { get; set; }
}