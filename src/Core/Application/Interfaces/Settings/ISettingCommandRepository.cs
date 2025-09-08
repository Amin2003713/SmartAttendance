using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Setting;

namespace SmartAttendance.Application.Interfaces.Settings;

public interface ISettingCommandRepository : ICommandRepository<Setting>,
    IScopedDependency;