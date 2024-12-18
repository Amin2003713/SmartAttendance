using Microsoft.AspNetCore.Identity;
using Shifty.Domain.Users;
using System;
using System.Threading.Tasks;

namespace Shifty.Persistence.Jwt
{
    public interface IJwtService<in TUser>  where TUser : IdentityUser<Guid>
    {
        Task<AccessToken> GenerateAsync(TUser user);
        Guid? ValidateJwtAccessTokenAsync(string token);
    }
}
