using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Vehicles;

namespace Shifty.Application.Interfaces.Vehicles;

public interface IVehicleCommandRepository : ICommandRepository<Vehicle>,
    IScopedDependency;