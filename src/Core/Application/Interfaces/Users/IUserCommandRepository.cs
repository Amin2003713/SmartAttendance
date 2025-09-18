using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Features.Users.Requests.Commands.RegisterByOwner;
using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.Application.Interfaces.Users;

public interface IUserCommandRepository : ICommandRepository<User>,
    IScopedDependency
{
    Task       UpdateLastLoginDateAsync(User                   user,    CancellationToken cancellationToken);
    Task<User> RegisterByOwnerAsync(RegisterByOwnerRequest     request, CancellationToken cancellationToken);
    Task       UpdateUserAsync(User                            user,    CancellationToken cancellationToken);
}