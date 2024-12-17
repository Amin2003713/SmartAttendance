using System;
using System.Collections.Generic;
using System.Linq;

namespace Shifty.Domain.Enums;

[Flags]
public enum UserRoles : byte
{
    None = 0,
    Admin = 1 << 0
    , // 1
    Employee = 1 << 1
    , // 2
    Driver = 1 << 2
    , // 4
    Accountants = 1 << 3 // 8
}

public static class UserRolesHelper
{
    // Check if a role is included
    public static bool HasRole(UserRoles roles, UserRoles roleToCheck) =>
        (roles & roleToCheck) == roleToCheck;

    // Add a role
    public static UserRoles AddRole(UserRoles roles, UserRoles roleToAdd) =>
        roles | roleToAdd;

    // Remove a role
    public static UserRoles RemoveRole(UserRoles roles, UserRoles roleToRemove) =>
        roles & ~roleToRemove;

    // List all roles in a combined flag
    public static List<UserRoles> GetRoles(UserRoles roles) =>
        Enum.GetValues(typeof(UserRoles)).Cast<UserRoles>().Where(r => r != UserRoles.None && HasRole(roles, r)).ToList();
}

