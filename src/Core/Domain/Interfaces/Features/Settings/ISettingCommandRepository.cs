using Shifty.Common;
using Shifty.Domain.Features.Setting;
using Shifty.Domain.Interfaces.Base;

namespace Shifty.Persistence.Repositories.Features.Settings.Queries;

public interface ISettingCommandRepository : IRepository<Setting> , IScopedDependency;