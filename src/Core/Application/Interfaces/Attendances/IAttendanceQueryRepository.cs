using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Attendances;

namespace SmartAttendance.Application.Interfaces.Attendances;

public interface IAttendanceQueryRepository : IQueryRepository<Attendance>,
    IScopedDependency { }