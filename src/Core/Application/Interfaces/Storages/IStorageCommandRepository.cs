using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Storages;

namespace SmartAttendance.Application.Interfaces.Storages;

public interface IStorageCommandRepository : ICommandRepository<Storage>,
    IScopedDependency { }