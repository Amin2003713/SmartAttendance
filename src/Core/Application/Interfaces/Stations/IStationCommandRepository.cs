using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Stations;

namespace Shifty.Application.Interfaces.Stations;

public interface IStationCommandRepository : ICommandRepository<Station>,
    IScopedDependency;