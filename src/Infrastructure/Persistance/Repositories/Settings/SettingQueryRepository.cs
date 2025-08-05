using Shifty.Application.Interfaces.Settings;

namespace Shifty.Persistence.Repositories.Settings;

public class SettingQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Setting>> logger
)
    : QueryRepository<Setting>(dbContext, logger),
        ISettingQueriesRepository;