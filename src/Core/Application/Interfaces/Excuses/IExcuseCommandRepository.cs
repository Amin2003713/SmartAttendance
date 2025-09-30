using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Excuses;

namespace SmartAttendance.Application.Interfaces.Excuses;

public interface IExcuseCommandRepository : ICommandRepository<Excuse>,
    IScopedDependency { }