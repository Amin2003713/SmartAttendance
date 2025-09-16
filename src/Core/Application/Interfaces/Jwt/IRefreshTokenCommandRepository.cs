using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.Application.Interfaces.Jwt;

public interface IRefreshTokenCommandRepository : ICommandRepository<UserToken>
{
    Task AddOrUpdateRefreshTokenAsync(UserToken refreshToken, CancellationToken cancellationToken);

    Task RevokeSecondarySession(List<UserToken> refreshTokens, CancellationToken cancellationToken);
}