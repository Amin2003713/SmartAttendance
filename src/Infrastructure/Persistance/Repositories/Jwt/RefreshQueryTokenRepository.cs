using SmartAttendance.Application.Interfaces.Jwt;
using SmartAttendance.Common.Utilities.InjectionHelpers;

namespace SmartAttendance.Persistence.Repositories.Jwt;

public class RefreshQueryTokenRepository(
    ReadOnlyDbContext                               dbContext,
    ILogger<RefreshQueryTokenRepository>            logger,
    IStringLocalizer<RefreshCommandTokenRepository> localizer,
    ILogger<QueryRepository<UserToken>>             writeOnlyLogger
)
    : QueryRepository<UserToken>(dbContext, writeOnlyLogger),
      IRefreshTokenQueryRepository,
      IScopedDependency
{
    public async Task<bool> ValidateRefreshTokenAsync(UserToken refreshToken, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Validating refresh token for UserId: {UserId}", refreshToken.UserId);

            var result = await TableNoTracking.SingleOrDefaultAsync(
                x => x.UserId == refreshToken.UserId && x.UniqueId == refreshToken.UniqueId,
                cancellationToken);

            if (result == null || result.RefreshToken != refreshToken.RefreshToken)
            {
                logger.LogWarning("Invalid refresh token for UserId: {UserId}", refreshToken.UserId);
                throw SmartAttendanceException.BadRequest(localizer["Invalid refresh token."]);
            }

            logger.LogInformation("Refresh token validated successfully for UserId: {UserId}", refreshToken.UserId);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while validating refresh token for UserId: {UserId}", refreshToken.UserId);
            throw SmartAttendanceException.InternalServerError(localizer["Failed to validate refresh token."]);
        }
    }

    public async Task<List<UserToken>> GetSessions(
        Guid              UserId,
        Guid              uniqueId,
        CancellationToken httpContextRequestAborted)
    {
        var result = await TableNoTracking.Where(a => a.UserId == UserId && a.UniqueId != uniqueId).ToListAsync(httpContextRequestAborted);

        return result;
    }

    public async Task<UserToken?> GetCurrentSessions(Guid uniqueId, CancellationToken httpContextRequestAborted)
    {
        return (await TableNoTracking.FirstOrDefaultAsync(a => a.UniqueId == uniqueId && a.IsActive,
                                                          httpContextRequestAborted))!;
    }
}