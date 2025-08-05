using Shifty.Application.Interfaces.Base;
using Shifty.Domain.Users;

namespace Shifty.Application.Interfaces.Users;

public interface IUserQueryRepository : IQueryRepository<User>;