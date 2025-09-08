using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Common.Utilities.RolesHelper;

public static class RoleDependencyHelper
{
    private readonly static Dictionary<Roles, (List<Roles> Add, List<Roles> Remove)> RoleDependencies = new()
    {
        [Roles.Admin] = (
            Enum.GetValues<Roles>().Where(r => r != Roles.Admin).ToList(),
            Enum.GetValues<Roles>().Where(r => r != Roles.Admin).ToList()
        ),
        [Roles.Messages_Create] = ([Roles.Messages_Read], []),
        [Roles.Messages_Edit] = ([Roles.Messages_Read], []),
        [Roles.Messages_Delete] = ([Roles.Messages_Read], []),
        [Roles.Messages_Read] = ([], [
            Roles.Messages_Create,
            Roles.Messages_Edit,
            Roles.Messages_Delete,
            Roles.ManageMessages
        ]),
        [Roles.ManageUsers] = ([Roles.Users_Create, Roles.Users_Edit, Roles.Users_Delete], [
            Roles.Users_Create,
            Roles.Users_Edit,
            Roles.Users_Delete,
            Roles.Admin
        ]),
        [Roles.ManageProjects] = (
            [Roles.Projects_Create, Roles.Projects_Edit, Roles.Projects_Delete, Roles.Projects_Archive], [
                Roles.Admin,
                Roles.Projects_Create,
                Roles.Projects_Edit,
                Roles.Projects_Delete,
                Roles.Projects_Archive
            ]),
        [Roles.ManageMessages] = (
            [Roles.Messages_Create, Roles.Messages_Edit, Roles.Messages_Delete, Roles.Messages_Read], [
                Roles.Admin,
                Roles.Messages_Create,
                Roles.Messages_Edit,
                Roles.Messages_Delete,
                Roles.Messages_Read
            ])
    };

    public static HashSet<Roles> GetAllAdditiveRoles(this Roles role)
    {
        var result  = new HashSet<Roles>();
        var visited = new HashSet<Roles>();
        ResolveAdditiveRoles(role, result, visited);
        return result;
    }

    public static HashSet<Roles> GetAllRemovableRoles(this Roles role)
    {
        var result  = new HashSet<Roles>();
        var visited = new HashSet<Roles>();
        ResolveRemovableRoles(role, result, visited);
        return result;
    }

    private static void ResolveAdditiveRoles(this Roles role, HashSet<Roles> result, HashSet<Roles> visited)
    {
        if (visited.Contains(role)) return;

        visited.Add(role);

        if (RoleDependencies.TryGetValue(role, out var dep))
            foreach (var addRole in dep.Add)
            {
                if (result.Add(addRole)) // avoid re-traversal if already added
                    ResolveAdditiveRoles(addRole, result, visited);
            }
    }

    private static void ResolveRemovableRoles(this Roles role, HashSet<Roles> result, HashSet<Roles> visited)
    {
        if (visited.Contains(role)) return;

        visited.Add(role);

        if (RoleDependencies.TryGetValue(role, out var dep))
            foreach (var removeRole in dep.Remove)
            {
                if (result.Add(removeRole))
                    ResolveRemovableRoles(removeRole, result, visited);
            }
    }
}