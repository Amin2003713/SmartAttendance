using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.Application.Interfaces.UserPasswords;

public interface IUserPasswordCommandRepository : ICommandRepository<UserPassword>,
                                                  IScopedDependency;