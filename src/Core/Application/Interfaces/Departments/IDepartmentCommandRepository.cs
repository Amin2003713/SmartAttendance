using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Departments;

namespace SmartAttendance.Application.Interfaces.Departments;

public interface IDepartmentCommandRepository : ICommandRepository<Department>,
    IScopedDependency;