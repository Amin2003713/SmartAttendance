using SmartAttendance.Common.Utilities.InjectionHelpers;

namespace SmartAttendance.Persistence.Repositories.Users;

public class UserQueryRepository(
    ReadOnlyDbContext              dbContext,
    ILogger<QueryRepository<User>> logger
)
    : QueryRepository<User>(dbContext, logger),
      IUserQueryRepository,
      IScopedDependency;