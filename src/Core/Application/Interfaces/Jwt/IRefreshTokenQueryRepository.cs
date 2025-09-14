using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.Application.Interfaces.Jwt;

public interface IRefreshTokenQueryRepository : IQueryRepository<UserToken>
{
    Task<bool>            ValidateRefreshTokenAsync(UserToken refreshToken, CancellationToken cancellationToken);
    Task<List<UserToken>> GetSessions(Guid                    UserId,       Guid              uniqueId, CancellationToken httpContextRequestAborted);
    Task<UserToken?>      GetCurrentSessions(Guid             uniqueId,     CancellationToken httpContextRequestAborted);
}