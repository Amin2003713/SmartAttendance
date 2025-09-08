using SmartAttendance.Application.Interfaces.Jwt;
using SmartAttendance.Common.Utilities.InjectionHelpers;

namespace SmartAttendance.Persistence.Repositories.Jwt;

public class RefreshCommandTokenRepository(
    WriteOnlyDbContext dbContext,
    IRefreshTokenQueryRepository queryRepository,
    ILogger<RefreshCommandTokenRepository> logger,
    ILogger<CommandRepository<UserToken>> writeOnlyLogger,
    IStringLocalizer<RefreshCommandTokenRepository> localizer
)
    : CommandRepository<UserToken>(dbContext, writeOnlyLogger),
        IRefreshTokenCommandRepository,
        IScopedDependency
{
    public async Task AddOrUpdateRefreshTokenAsync(UserToken refreshToken, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Adding or updating refresh token for UserId: {UserId}", refreshToken.UserId);


            refreshToken.CreatedBy = refreshToken.UserId;
            await AddAsync(refreshToken, cancellationToken);
            logger.LogInformation("Refresh token created for UserId: {UserId}", refreshToken.UserId);


            var oldSessions =
                await queryRepository.GetSessions(refreshToken.UserId, refreshToken.UniqueId, cancellationToken);

            if (oldSessions.Count > 0)
                await RevokeSecondarySession(oldSessions, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in AddOrUpdateRefreshTokenAsync for UserId: {UserId}", refreshToken.UserId);
            throw SmartAttendanceException.InternalServerError(localizer["Failed to process refresh token."]);
        }
    }

    public async Task RevokeSecondarySession(List<UserToken> refreshTokens, CancellationToken cancellationToken)
    {
        await DeleteRangeAsync(refreshTokens, cancellationToken);
    }
}