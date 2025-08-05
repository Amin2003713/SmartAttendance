using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Setting;

namespace Shifty.Application.Interfaces.Settings;

public interface ISettingCommandRepository : ICommandRepository<Setting>,
    IScopedDependency;