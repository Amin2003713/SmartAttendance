using SmartAttendance.Application.Interfaces.UserPasswords;

namespace SmartAttendance.Persistence.Repositories.UserPasswords;

public class UserPasswordQueryRepository(
    ReadOnlyDbContext                      dbContext,
    ILogger<QueryRepository<UserPassword>> logger
)
    : QueryRepository<UserPassword>(dbContext, logger),
        IUserPasswordQueryRepository;