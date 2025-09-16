namespace SmartAttendance.Application.Interfaces.UserPasswords;

public interface IUserPasswordCommandRepository : ICommandRepository<UserPassword>,
                                                  IScopedDependency;