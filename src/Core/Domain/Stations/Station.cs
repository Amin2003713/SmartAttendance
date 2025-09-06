using Shifty.Common.Common.Requests;
using Shifty.Common.General.BaseClasses;
using Shifty.Common.General.Enums.StationStatuses;
using Shifty.Common.General.Enums.StationTypes;

namespace Shifty.Domain.Stations;

public class Station : BaseEntity
{
    public string Name { get; set; }

    public string Code { get; set; }

    public decimal AllowedDistance { get; set; }

    public TimeSpan OnWay { get; set; }

    public Location Location { get; set; }

    public StationStatus StationStatus { get; set; }

    public StationType StationType { get; set; }

    // public virtual ICollection<Missions> Missions { get; set; }


    public void Update(Station station)
    {
        Name = station.Name;
        Code = station.Code;
        AllowedDistance = station.AllowedDistance;
        OnWay = station.OnWay;
        Location = station.Location;
        StationStatus = station.StationStatus;
        StationType = station.StationType;
    }
}