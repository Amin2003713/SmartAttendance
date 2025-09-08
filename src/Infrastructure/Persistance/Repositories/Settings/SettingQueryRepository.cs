using SmartAttendance.Application.Interfaces.Settings;

namespace SmartAttendance.Persistence.Repositories.Settings;

public class SettingQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Setting>> logger
)
    : QueryRepository<Setting>(dbContext, logger),
        ISettingQueriesRepository;