using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Application.Features.Missions.Requests.Commands.Create;

public class CreateMissionRequest
{
    public Guid EmployeeId { get; set; }
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
}