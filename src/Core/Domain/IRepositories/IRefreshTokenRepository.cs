using Shifty.Domain.Entities.Users;
using Shifty.Domain.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Domain.IRepositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task AddOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task<bool> ValidateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    }
}
