using Shifty.Application.Interfaces.Settings;

namespace Shifty.Persistence.Repositories.Settings;

public class SettingCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Setting>> logger
)
    : CommandRepository<Setting>(dbContext, logger),
        ISettingCommandRepository;