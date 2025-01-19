using System;
using System.Collections.Generic;
using System.Linq;

namespace Shifty.Domain.Enums
{
    public static class UserRolesHelper
    {
        // Check if a role is included
        public static bool HasRole(UserRoles roles , UserRoles roleToCheck)
        {
            return (roles & roleToCheck) == roleToCheck;
        }

        // Add a role
        public static UserRoles AddRole(UserRoles roles , UserRoles roleToAdd)
        {
            return roles | roleToAdd;
        }

        // Remove a role
        public static UserRoles RemoveRole(UserRoles roles , UserRoles roleToRemove)
        {
            return roles & ~roleToRemove;
        }

        // List all roles in a combined flag
        public static List<UserRoles> GetRoles(UserRoles roles)
        {
            return Enum.GetValues(typeof(UserRoles)).Cast<UserRoles>().Where(r => r != UserRoles.None && HasRole(roles , r)).ToList();
        }
    }
}