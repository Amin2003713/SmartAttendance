using SmartAttendance.Application.Features.Users.Requests.Commands.RegisterByOwner;
using SmartAttendance.Application.Features.Users.Requests.Commands.UpdatePhoneNumber;

namespace SmartAttendance.Application.Interfaces.Users;

public interface IUserCommandRepository : ICommandRepository<User>,
    IScopedDependency
{
    Task       UpdateLastLoginDateAsync(User                   user,    CancellationToken cancellationToken);
    Task<Guid> RegisterByOwnerAsync(RegisterByOwnerRequest     request, CancellationToken cancellationToken);
    Task       UpdatePhoneNumberAsync(UpdatePhoneNumberRequest request, Guid              userId, CancellationToken cancellationToken);
    Task       UpdateUserAsync(User                            user,    CancellationToken cancellationToken);
}