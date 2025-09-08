using SmartAttendance.Application.Interfaces.Settings;

namespace SmartAttendance.Persistence.Repositories.Settings;

public class SettingCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Setting>> logger
)
    : CommandRepository<Setting>(dbContext, logger),
        ISettingCommandRepository;