using Shifty.Application.Interfaces.UserPasswords;

namespace Shifty.Persistence.Repositories.UserPasswords;

public class UserPasswordQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<UserPassword>> logger
)
    : QueryRepository<UserPassword>(dbContext, logger),
        IUserPasswordQueryRepository;