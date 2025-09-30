using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Application.Interfaces.Attendances;

public interface IAttendanceCommandRepository : ICommandRepository<Attendance>,
    IScopedDependency { }