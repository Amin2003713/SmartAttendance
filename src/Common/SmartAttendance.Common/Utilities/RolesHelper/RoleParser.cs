using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Common.Utilities.RolesHelper;

public static class RoleParser
{
    public readonly static HashSet<string> ValidRoleNames = Enum.GetNames(typeof(RolesType)).ToHashSet(StringComparer.OrdinalIgnoreCase);

    public static bool IsValid(string roleName)
    {
        return ValidRoleNames.Contains(roleName);
    }

    public static RolesType Parse(string roleName)
    {
        if (Enum.TryParse<RolesType>(roleName, true, out var result))
            return result;

        throw new ArgumentException($"Invalid role name: {roleName}");
    }

    public static List<RolesType> ParseMany(List<string> inputRoles)
    {
        var invalid = inputRoles.Where(r => !IsValid(r)).ToList();

        if (invalid.Any())
            throw new ArgumentException($"Invalid roles: {string.Join(", ", invalid)}");

        return inputRoles.Select(Parse).Distinct().ToList();
    }
}