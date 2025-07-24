using Shifty.Application.Commons.Base;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Users;

namespace Shifty.Application.Interfaces.UserPasswords;

public interface IUserPasswordCommandRepository : ICommandRepository<UserPassword>,
    IScopedDependency;