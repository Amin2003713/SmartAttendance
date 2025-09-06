using Shifty.Common.Common.Requests;
using Shifty.Common.General.BaseClasses;
using Shifty.Common.General.Enums;

namespace Shifty.Domain.Vehicles;

public class Vehicle : BaseEntity
{
    public required string Title { get; set; }

    public required PlateNumber PlateNumber { get; set; }

    public VehicleType VehicleType { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public DateTime YearOfProduction { get; set; }
    public Guid ResponsibleId { get; set; }
    public string Descriprion { get; set; }
    public VehicleStatus Status { get; set; }


    // public virtual ICollection<Missions> Missions { get; set; }


    public void Update(Vehicle vehicle)
    {
        Title = vehicle.Title;
        PlateNumber = vehicle.PlateNumber;
        VehicleType = vehicle.VehicleType;
        Brand = vehicle.Brand;
        Model = vehicle.Model;
        YearOfProduction = vehicle.YearOfProduction;
        ResponsibleId = vehicle.ResponsibleId;
        Descriprion = vehicle.Descriprion;
        Status = vehicle.Status;
    }
}