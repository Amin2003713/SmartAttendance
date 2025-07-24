using Shifty.Application.Interfaces.UserPasswords;

namespace Shifty.Persistence.Repositories.UserPasswords;

public class UserPasswordCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<UserPassword>> logger
)
    : CommandRepository<UserPassword>(dbContext, logger),
        IUserPasswordCommandRepository;