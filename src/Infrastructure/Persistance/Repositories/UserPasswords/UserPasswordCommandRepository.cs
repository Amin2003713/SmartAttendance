using SmartAttendance.Application.Interfaces.UserPasswords;

namespace SmartAttendance.Persistence.Repositories.UserPasswords;

public class UserPasswordCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<UserPassword>> logger
)
    : CommandRepository<UserPassword>(dbContext, logger),
        IUserPasswordCommandRepository;