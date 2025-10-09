using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Majors;

namespace SmartAttendance.Application.Interfaces.Majors;

public interface IMajorCommandRepository : ICommandRepository<Major>,
    IScopedDependency { }