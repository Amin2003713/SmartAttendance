using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Domain.Missions.Events.TaskTrackers;

public record MissionCreatedEvent(
    Guid AggregateId,
    Guid EmployeeId,
    DateTime StartDate,
    DateTime EndDate,
    string Destination,
    decimal Salary,
    MissionType MissionType,
    MissionVehicleType MissionVehicleType,
    bool HotelReservation,
    bool AdvancePayment,
    bool UrgentMission,
    string Description
) : DomainEvent;