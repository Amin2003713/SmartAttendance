using System;
using System.Threading.Tasks;
using Shifty.Domain.Features.Users;

namespace Shifty.Persistence.Jwt
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
        Guid? ValidateJwtAccessTokenAsync(string token);
    }
}