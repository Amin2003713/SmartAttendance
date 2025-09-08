using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Vehicles;

namespace SmartAttendance.Application.Interfaces.Vehicles;

public interface IVehicleQueryRepository : IQueryRepository<Vehicle>,
    IScopedDependency;