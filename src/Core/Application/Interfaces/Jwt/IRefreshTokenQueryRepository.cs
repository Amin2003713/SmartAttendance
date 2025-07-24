using System.Threading;
using System.Threading.Tasks;
using Shifty.Application.Commons.Base;
using Shifty.Domain.Users;

namespace Shifty.Application.Interfaces.Jwt;

public interface IRefreshTokenQueryRepository : IQueryRepository<UserToken>
{
    Task<bool>            ValidateRefreshTokenAsync(UserToken refreshToken, CancellationToken cancellationToken);
    Task<List<UserToken>> GetSessions(Guid UserId, Guid uniqueId, CancellationToken httpContextRequestAborted);
    Task<UserToken?>      GetCurrentSessions(Guid uniqueId, CancellationToken httpContextRequestAborted);
}