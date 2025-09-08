namespace SmartAttendance.Persistence.Jwt;

public interface IJwtService
{
    Task<AccessToken>                GenerateAsync(User user, string uniqueId);
    Task<(Guid? userId, Guid? uniq)> ValidateJwtAccessTokenAsync(string token);
}