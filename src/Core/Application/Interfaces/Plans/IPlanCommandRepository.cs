using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Application.Interfaces.Plans;

public interface IPlanCommandRepository : ICommandRepository<Plan>,
    IScopedDependency { }