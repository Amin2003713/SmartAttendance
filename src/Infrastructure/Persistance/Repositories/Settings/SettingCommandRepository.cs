using Shifty.Application.Interfaces.Settings;
using Shifty.Domain.Setting;

namespace Shifty.Persistence.Repositories.Settings;

public class SettingCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Setting>> logger
)
    : CommandRepository<Setting>(dbContext, logger),
        ISettingCommandRepository;