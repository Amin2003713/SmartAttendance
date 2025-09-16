namespace SmartAttendance.Application.Interfaces.UserPasswords;

public interface IUserPasswordQueryRepository : IQueryRepository<UserPassword>,
    IScopedDependency;