using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Common.Utilities.RolesHelper;

public static class RoleDependencyHelper
{
    private readonly static Dictionary<RolesType, (List<RolesType> Add, List<RolesType> Remove)> RoleDependencies
        = new()
        {
            [RolesType.Admin] = (
                Enum.GetValues<RolesType>().Where(r => r != RolesType.Admin).ToList(),
                Enum.GetValues<RolesType>().Where(r => r != RolesType.Admin).ToList()
            ),
            [RolesType.Messages_Create] = ([RolesType.Messages_Read], []),
            [RolesType.Messages_Edit]   = ([RolesType.Messages_Read], []),
            [RolesType.Messages_Delete] = ([RolesType.Messages_Read], []),
            [RolesType.Messages_Read] = ([], [
                RolesType.Messages_Create,
                RolesType.Messages_Edit,
                RolesType.Messages_Delete,
                RolesType.ManageMessages
            ]),
            [RolesType.ManageUsers] = ([RolesType.Users_Create, RolesType.Users_Edit, RolesType.Users_Delete], [
                RolesType.Users_Create,
                RolesType.Users_Edit,
                RolesType.Users_Delete,
                RolesType.Admin
            ]),
            [RolesType.ManageProjects] = (
                [RolesType.Projects_Create, RolesType.Projects_Edit, RolesType.Projects_Delete, RolesType.Projects_Archive], [
                    RolesType.Admin,
                    RolesType.Projects_Create,
                    RolesType.Projects_Edit,
                    RolesType.Projects_Delete,
                    RolesType.Projects_Archive
                ]),
            [RolesType.ManageMessages] = (
                [RolesType.Messages_Create, RolesType.Messages_Edit, RolesType.Messages_Delete, RolesType.Messages_Read], [
                    RolesType.Admin,
                    RolesType.Messages_Create,
                    RolesType.Messages_Edit,
                    RolesType.Messages_Delete,
                    RolesType.Messages_Read
                ])
        };

    public static HashSet<RolesType> GetAllAdditiveRoles(this RolesType role)
    {
        var result  = new HashSet<RolesType>();
        var visited = new HashSet<RolesType>();
        ResolveAdditiveRoles(role, result, visited);
        return result;
    }

    public static HashSet<RolesType> GetAllRemovableRoles(this RolesType role)
    {
        var result  = new HashSet<RolesType>();
        var visited = new HashSet<RolesType>();
        ResolveRemovableRoles(role, result, visited);
        return result;
    }

    private static void ResolveAdditiveRoles(this RolesType role, HashSet<RolesType> result, HashSet<RolesType> visited)
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

    private static void ResolveRemovableRoles(this RolesType role, HashSet<RolesType> result, HashSet<RolesType> visited)
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