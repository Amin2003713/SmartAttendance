using Shifty.Domain.Users;
using System;
using System.Threading.Tasks;

namespace Shifty.Persistence.Jwt
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
        Guid? ValidateJwtAccessTokenAsync(string token);
    }
}
