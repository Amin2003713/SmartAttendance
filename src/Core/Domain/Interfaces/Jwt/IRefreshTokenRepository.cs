using Shifty.Domain.Interfaces.Base;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Domain.Features.Users;

namespace Shifty.Domain.Interfaces.Jwt
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task AddOrUpdateRefreshTokenAsync(RefreshToken refreshToken , CancellationToken cancellationToken);
        Task<bool> ValidateRefreshTokenAsync(RefreshToken refreshToken , CancellationToken cancellationToken);
    }
}