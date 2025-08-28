using Shifty.Common.Common.Requests;
using Shifty.Common.General.BaseClasses;

namespace Shifty.Domain.Vehicles;

public class Vehicle : BaseEntity
{
    public required string Title { get; set; }

    public required PlateNumber PlateNumber { get; set; }

    // public virtual ICollection<Mission> Missions { get; set; }


    public void Update(Vehicle vehicle)
    {
        Title = vehicle.Title;
        PlateNumber = vehicle.PlateNumber;
    }
}