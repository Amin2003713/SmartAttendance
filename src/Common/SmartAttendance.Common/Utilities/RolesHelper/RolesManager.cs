using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Common.Utilities.RolesHelper;

public static class RolesManager
{
    public const   string DeleteProject = $"{nameof(Roles.ManageProjects)},{nameof(Roles.Projects_Delete)},{nameof(Roles.Admin)},{nameof(Support)}";
    private static string Support       = nameof(Roles.Support);
}