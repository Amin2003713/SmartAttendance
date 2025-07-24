using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Commons.Base;
using Shifty.Domain.Users;

namespace Shifty.Application.Interfaces.Jwt;

public interface IRefreshTokenCommandRepository : ICommandRepository<UserToken>
{
    Task AddOrUpdateRefreshTokenAsync(UserToken refreshToken, CancellationToken cancellationToken);

    Task RevokeSecondarySession(List<UserToken> refreshTokens, CancellationToken cancellationToken);
}