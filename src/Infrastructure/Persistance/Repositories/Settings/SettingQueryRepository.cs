using Shifty.Application.Interfaces.Settings;
using Shifty.Domain.Setting;

namespace Shifty.Persistence.Repositories.Settings;

public class SettingQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Setting>> logger
)
    : QueryRepository<Setting>(dbContext, logger),
        ISettingQueriesRepository;