using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Stations;
using Shifty.Domain.Vehicles;

namespace Shifty.Application.Interfaces.Vehicles;

public interface IVehicleQueryRepository : IQueryRepository<Vehicle>,
    IScopedDependency;