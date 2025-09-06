using Newtonsoft.Json;
using Shifty.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using Shifty.Common.General.Enums;

namespace Shifty.Application.Features.Missions.Requests.Queries.MissionResponse;

public class GetMissionResponse
{
    public Guid AggregateId { get; set; }

    public LogPropertyInfoResponse? Employee { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string Destination { get; set; }

    public decimal Salary { get; set; }
    public string Description { get; set; }

    public MissionType MissionType { get; set; }

    public MissionVehicleType VehicleType { get; set; }

    public bool HotelReservation { get; set; }

    public bool AdvancePayment { get; set; }

    public bool UrgentMission { get; set; }

    [JsonIgnore] public DateTime Reported { get; set; }
}