using Shifty.Common.General.Enums;
using Shifty.Domain.TaskTracks;

namespace Shifty.Domain.Missions.SnapShots;

public class MissionSnapShot : ISnapshot<Guid>
{
    public Guid EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string Destination { get; set; }

    public decimal Salary { get; set; }
    public string Description { get; set; }

    public MissionType MissionType { get; set; }

    public MissionVehicleType MissionVehicleType { get; set; }

    public bool HotelReservation { get; set; }

    public bool AdvancePayment { get; set; }

    public bool UrgentMission { get; set; }
    public DateTime Reported { get; set; }

    public Guid AggregateId { get; init; }
    public int Version { get; init; }
    public DateTime LastAction { get; init; }
    public bool Deleted { get; init; }
}