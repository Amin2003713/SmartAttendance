using Microsoft.EntityFrameworkCore;
using Shifty.Common;
using Shifty.Domain.IRepositories;
using Shifty.Domain.Users;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;
using Shifty.Persistence.TenantServices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.Repositories.Jwt
{
    public class RefreshTokenRepository(WriteOnlyDbContext dbContext) : Repository<RefreshToken>(dbContext), IRefreshTokenRepository, IScopedDependency
    {
        public async Task AddOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            var token = await TableNoTracking.SingleOrDefaultAsync(x => x.UserId == refreshToken.UserId, cancellationToken);
            if (token == null)
            {
                refreshToken.CreatedAt = DateTime.Now;
                await base.AddAsync(refreshToken, cancellationToken);
            }
            else
            {
                refreshToken.CreatedAt = token.CreatedAt;
                refreshToken.Id = token.Id;
                await base.UpdateAsync(refreshToken, cancellationToken);
            }
        }
        public async Task<bool> ValidateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            var result = await TableNoTracking.SingleOrDefaultAsync(x => x.UserId == refreshToken.UserId, cancellationToken);
            if (result == null || result.Token != refreshToken.Token)
                throw new ShiftyException("RefreshToken is not valid");
            return true;
        }
    }
}
