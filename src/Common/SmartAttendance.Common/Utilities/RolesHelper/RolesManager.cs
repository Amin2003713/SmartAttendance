using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Common.Utilities.RolesHelper;

public static class RolesManager
{
    public const   string DeleteProject = $"{nameof(RolesType.ManageProjects)},{nameof(RolesType.Projects_Delete)},{nameof(RolesType.Admin)},{nameof(Support)}";
    private static string Support       = nameof(RolesType.Support);
}