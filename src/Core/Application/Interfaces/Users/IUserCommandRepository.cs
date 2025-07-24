using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Commons.Base;
using Shifty.Application.Users.Requests.Commands.RegisterByOwner;
using Shifty.Application.Users.Requests.Commands.UpdatePhoneNumber;
using Shifty.Common.InjectionHelpers;
using Shifty.Domain.Users;

namespace Shifty.Application.Interfaces.Users;

public interface IUserCommandRepository : ICommandRepository<User>,
    IScopedDependency
{
    Task       UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
    Task<Guid> RegisterByOwnerAsync(RegisterByOwnerRequest request, CancellationToken cancellationToken);
    Task       UpdatePhoneNumberAsync(UpdatePhoneNumberRequest request, Guid userId, CancellationToken cancellationToken);
    Task       UpdateUserAsync(User user, CancellationToken cancellationToken);
}