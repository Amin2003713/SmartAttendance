using Shifty.Common.General.BaseClasses;
using Shifty.Common.General.Enums.Stationstatuses;
using Shifty.Common.General.Enums.StationTypes;
using Shifty.Domain.Locations;

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

    // public virtual ICollection<Mission> Missions { get; set; }
}