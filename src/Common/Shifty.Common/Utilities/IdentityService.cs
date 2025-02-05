using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Shifty.Common.Utilities;

public class IdentityService(IHttpContextAccessor accessor) : IScopedDependency
{
    private readonly ClaimsPrincipal       _identity = accessor.HttpContext?.User;

    public string FindFirstValue(string claimType)
    {
        return _identity?.FindFirst(claimType)?.Value;
    }

    public string GetValue(Predicate<Claim> predicate)
    {
        return _identity?.FindFirst(predicate)?.Value ?? string.Empty;
    }

    public Guid? GetUserId()
        {
            if (_identity == null || _identity.Identities == null)
            {
                return null;
            }

            var userIdClaim = _identity.Identities.Select(identity => identity.FindFirst("id")).FirstOrDefault();

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value , out var id) || id == Guid.Empty)
            {
                return null;
            }

            return id;
        }
    

    public T GetUserId<T>() where T : IConvertible
    {
        var userId = GetUserId();
        return userId.HasValue ? (T)Convert.ChangeType(userId , typeof(T) , CultureInfo.InvariantCulture) : default;
    }

    public string GetUserName()
    {
        return _identity?.Identities.Select(identity => identity.FindFirst("username")).FirstOrDefault()!.Value;
    }

    public string GetFirstName()
    {
        return _identity?.Identities.Select(identity => identity.FindFirst("firstName")).FirstOrDefault()!.Value;

    }

    public string GetLastName()
    {
       return _identity?.Identities.Select(identity => identity.FindFirst("lastName")).FirstOrDefault()!.Value;
    }

    public string GetProfile()
    {
        return _identity?.Identities.Select(identity => identity.FindFirst("profile")).FirstOrDefault()!.Value;
    }

    public DateTime? GetLastLoginDate()
    {
        var lastLoginDate = _identity?.Identities.Select(identity => identity.FindFirst("lastLoginDate")).FirstOrDefault()!.Value;
        return DateTime.TryParse(lastLoginDate , out var date) ? date : null;
    }

    public List<string> GetRoles()
    {
        return _identity?.FindAll(ClaimTypes.Role)?.Select(c => c.Value).ToList() ?? [];
    }

    public string GetCustomClaim(string claimType)
    {
        return _identity?.FindFirst(claimType)?.Value;
    }
}

