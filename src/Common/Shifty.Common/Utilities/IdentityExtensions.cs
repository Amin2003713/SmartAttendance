using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Shifty.Common.Utilities;

public static class IdentityExtensions
{
    public static string FindFirstValue(this ClaimsIdentity identity , string claimType)
    {
        return identity?.FindFirst(claimType)?.Value;
    }

    public static string FindFirstValue(this IIdentity identity , string claimType)
    {
        var claimsIdentity = identity as ClaimsIdentity;
        return claimsIdentity?.FindFirstValue(claimType);
    }

    public static string GetValue(this IIdentity identity , Predicate<Claim> predicate)
    {
        var claimsIdentity = identity as ClaimsIdentity;
        return claimsIdentity?.FindFirst(predicate)!.Value ?? string.Empty;
    }

    public static string GetUserId(this IIdentity identity)
    {
        return identity?.FindFirstValue("id");
    }

    public static T GetUserId<T>(this IIdentity identity) where T : IConvertible
    {
        var userId = identity?.GetUserId();
        return userId.HasValue() ? (T)Convert.ChangeType(userId , typeof(T) , CultureInfo.InvariantCulture) : default;
    }

    public static string GetUserName(this IIdentity identity)
    {
        return identity?.FindFirstValue("username");
    }



}