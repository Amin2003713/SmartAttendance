namespace SmartAttendance.Application.Interfaces.Jwt;

public interface IRefreshTokenCommandRepository : ICommandRepository<UserToken>
{
    Task AddOrUpdateRefreshTokenAsync(UserToken refreshToken, CancellationToken cancellationToken);

    Task RevokeSecondarySession(List<UserToken> refreshTokens, CancellationToken cancellationToken);
}