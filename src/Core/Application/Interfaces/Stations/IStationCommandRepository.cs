using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Stations;

namespace SmartAttendance.Application.Interfaces.Stations;

public interface IStationCommandRepository : ICommandRepository<Station>,
    IScopedDependency;