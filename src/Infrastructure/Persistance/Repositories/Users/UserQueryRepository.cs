using Shifty.Common.Utilities.InjectionHelpers;

namespace Shifty.Persistence.Repositories.Users;

public class UserQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<User>> logger
)
    : QueryRepository<User>(dbContext, logger),
        IUserQueryRepository,
        IScopedDependency;