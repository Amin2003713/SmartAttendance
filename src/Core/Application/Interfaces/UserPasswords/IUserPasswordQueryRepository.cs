using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Users;

namespace Shifty.Application.Interfaces.UserPasswords;

public interface IUserPasswordQueryRepository : IQueryRepository<UserPassword>,
    IScopedDependency;