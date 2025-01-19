using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Shifty.Persistence.Services.UserClaimsService
{
    public static class ClaimsPrincipalExtensions
    {
        // Get the user ID from the claims
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