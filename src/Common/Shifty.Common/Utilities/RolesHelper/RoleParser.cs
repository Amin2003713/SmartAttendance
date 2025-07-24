using Shifty.Common.General.Enums;

namespace Shifty.Common.Utilities.RolesHelper;

public static class RoleParser
{
    public readonly static HashSet<string> ValidRoleNames = Enum
        .GetNames(typeof(Roles))
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    public static bool IsValid(string roleName)
    {
        return ValidRoleNames.Contains(roleName);
    }

    public static Roles Parse(string roleName)
    {
        if (Enum.TryParse<Roles>(roleName, true, out var result))
            return result;

        throw new ArgumentException($"Invalid role name: {roleName}");
    }

    public static List<Roles> ParseMany(List<string> inputRoles)
    {
        var invalid = inputRoles
            .Where(r => !IsValid(r))
            .ToList();

        if (invalid.Any())
            throw new ArgumentException($"Invalid roles: {string.Join(", ", invalid)}");

        return inputRoles
            .Select(Parse)
            .Distinct()
            .ToList();
    }
}