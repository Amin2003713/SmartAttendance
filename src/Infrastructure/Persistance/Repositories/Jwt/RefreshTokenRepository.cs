using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shifty.Common;
using Shifty.Domain.Interfaces.Base;
using Shifty.Domain.Interfaces.Jwt;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;
using Shifty.Resources.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Common.Exceptions;
using Shifty.Domain.Features.Users;

namespace Shifty.Persistence.Repositories.Jwt
{
    public class RefreshTokenRepository(WriteOnlyDbContext dbContext , ILogger<RefreshTokenRepository> logger , ILogger<Repository<RefreshToken , WriteOnlyDbContext>> writeOnlyLogger , CommonMessages messages)
        : Repository<RefreshToken , WriteOnlyDbContext>(dbContext , writeOnlyLogger) , IRefreshTokenRepository , IScopedDependency
    {
        public async Task AddOrUpdateRefreshTokenAsync(RefreshToken refreshToken , CancellationToken cancellationToken)
        {
            try
            {
                var token = await TableNoTracking.SingleOrDefaultAsync(x => x.UserId == refreshToken.UserId , cancellationToken);
                if (token == null)
                {
                    refreshToken.CreatedAt = DateTime.Now;
                    await base.AddAsync(refreshToken , cancellationToken);
                }
                else
                {
                    refreshToken.CreatedAt = token.CreatedAt;
                    refreshToken.Id        = token.Id;
                    await base.UpdateAsync(refreshToken , cancellationToken);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.Source , e);
                throw ShiftyException.InternalServerError();
            }
        }

        public async Task<bool> ValidateRefreshTokenAsync(RefreshToken refreshToken , CancellationToken cancellationToken)
        {
            var result = await TableNoTracking.SingleOrDefaultAsync(x => x.UserId == refreshToken.UserId , cancellationToken);
            if (result == null || result.Token != refreshToken.Token)
                throw  ShiftyException.BadRequest(messages.Refresh_Token_Found());
            return true;
        }
    }
}