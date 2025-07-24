using Shifty.Application.Commons.Base;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Setting;

namespace Shifty.Application.Interfaces.Settings;

public interface ISettingCommandRepository : ICommandRepository<Setting>,
    IScopedDependency;