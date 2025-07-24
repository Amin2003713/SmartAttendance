using Shifty.Common.General.Enums;

namespace Shifty.Common.Utilities.RolesHelper;

public static class RolesManager
{
    public const   string DeleteProject = $"{nameof(Roles.ManageProjects)},{nameof(Roles.Projects_Delete)},{nameof(Roles.Admin)},{nameof(Support)}";
    private static string Support       = nameof(Roles.Support);
}