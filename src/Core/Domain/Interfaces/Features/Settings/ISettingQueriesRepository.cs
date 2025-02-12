using Shifty.Common;
using Shifty.Domain.Features.Setting;
using Shifty.Domain.Interfaces.Base;

namespace Shifty.Domain.Interfaces.Features.Settings;

public interface ISettingQueriesRepository :  IRepository<Setting>  , IScopedDependency{}