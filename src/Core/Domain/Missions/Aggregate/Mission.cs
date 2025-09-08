using Mapster;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Domain.Missions.Events.TaskTrackers;
using SmartAttendance.Domain.Missions.SnapShots;

namespace SmartAttendance.Domain.Missions.Aggregate;

public class Mission : AggregateRoot<Guid>
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

    [JsonIgnore] public DateTime Reported { get; set; }


#region Factory

    public static Mission New(MissionCreatedEvent @event)
    {
        var task = new Mission();
        task.Create(@event);
        return task;
    }

#endregion

#region Behavioral Methods

    public void Create(MissionCreatedEvent @event)
    {
        RaiseEvent(@event);
    }

    public void Update(MissionUpdatedEvent @event)
    {
        RaiseEvent(@event);
    }

    public void Delete(MissionDeletedEvent @event)
    {
        RaiseEvent(@event);
    }

#endregion

#region Apply Methods

    public void Apply(MissionCreatedEvent @event)
    {
        AggregateId = @event.AggregateId;
        EmployeeId = @event.EmployeeId;
        AdvancePayment = @event.AdvancePayment;
        UrgentMission = @event.UrgentMission;
        Destination = @event.Destination;
        Salary = @event.Salary;
        HotelReservation = @event.HotelReservation;
        MissionType = @event.MissionType;
        MissionVehicleType = @event.MissionVehicleType;
        Description = @event.Description;
        EndDate = @event.EndDate;
        StartDate = @event.StartDate;
        Reported = @event.Reported;
    }

    public void Apply(MissionUpdatedEvent @event)
    {
        AggregateId = @event.AggregateId;
        EmployeeId = @event.EmployeeId;
        AdvancePayment = @event.AdvancePayment;
        UrgentMission = @event.UrgentMission;
        Destination = @event.Destination;
        Salary = @event.Salary;
        HotelReservation = @event.HotelReservation;
        MissionType = @event.MissionType;
        MissionVehicleType = @event.MissionVehicleType;
        Description = @event.Description;
        EndDate = @event.EndDate;
        StartDate = @event.StartDate;
    }

    public void Apply(MissionDeletedEvent @event)
    {
        AggregateId = @event.AggregateId;
        Deleted = true;
    }

#endregion

#region Snapshot

    protected override void RestoreFromSnapshot(ISnapshot<Guid> snapshot)
    {
        if (snapshot is not MissionSnapShot s)
            throw new InvalidCastException("Invalid snapshot type");

        AggregateId = s.AggregateId;
        Version = s.Version;
        Reported = s.Reported;
        Deleted = s.Deleted;
        Description = s.Description;
        StartDate = s.StartDate;
        EndDate = s.EndDate;
        AdvancePayment = s.AdvancePayment;
        UrgentMission = s.UrgentMission;
        Destination = s.Destination;
        Salary = s.Salary;
        HotelReservation = s.HotelReservation;
        MissionType = s.MissionType;
        MissionVehicleType = s.MissionVehicleType;
        EmployeeId = s.EmployeeId;
    }


    public override ISnapshot<Guid> GetSnapshot()
    {
        return this.Adapt<MissionSnapShot>();
    }

#endregion
}