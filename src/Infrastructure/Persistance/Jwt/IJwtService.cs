using Microsoft.AspNetCore.Identity;
using Shifty.Domain.Tenants;
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


    public interface IJwtServiceForTenant
    {
        Task<AccessToken> GenerateAsync(TenantAdmin user);
        Guid? ValidateJwtAccessTokenAsync(string token);
    }
}
