using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Shifty.Common.Utilities
{
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
            return identity?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static T GetUserId<T>(this IIdentity identity) where T : IConvertible
        {
            var userId = identity?.GetUserId();
            return userId.HasValue() ? (T)Convert.ChangeType(userId , typeof(T) , CultureInfo.InvariantCulture) : default;
        }

        public static string GetUserName(this IIdentity identity)
        {
            return identity?.FindFirstValue(ClaimTypes.Name);
        }

        public static Guid GetUserId()
        {
            return Guid.Parse(ClaimsPrincipal.Current?.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }

        // Get the email from the claims
        public static string GetEmail()
        {
            return ClaimsPrincipal.Current?.FindFirst(ClaimTypes.Email)?.Value;
        }

        // Get the ClaimsPrincipal.Currentname from the claims
        public static string GetUsername()
        {
            return ClaimsPrincipal.Current?.FindFirst(ClaimTypes.Name)?.Value;
        }

        // Get all roles from the claims
        public static List<string> GetRoles()
        {
            return ClaimsPrincipal.Current?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        }

        // Get a custom claim by type
        public static string GetCustomClaim(string claimType)
        {
            return ClaimsPrincipal.Current?.FindFirst(claimType)?.Value;
        }
    }
}