using SmartAttendance.Common.Common.Requests;
using SmartAttendance.Common.General.Enums.StationStatuses;
using SmartAttendance.Common.General.Enums.StationTypes;

namespace SmartAttendance.Application.Features.Stations.Requests.Queries.GetStations;

public class GetStationResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public decimal AllowedDistance { get; set; }

    public TimeSpan OnWay { get; set; }

    public Location Location { get; set; }

    public StationStatus StationStatus { get; set; }

    public StationType StationType { get; set; }
}