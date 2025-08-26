using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Features.Users.Requests.Commands.RegisterByOwner;
using Shifty.Application.Features.Users.Requests.Commands.UpdatePhoneNumber;
using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
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