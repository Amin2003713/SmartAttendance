namespace SmartAttendance.Common.Utilities.EnumHelpers;

public static class AccessHelper
{
    // /// <summary>
    // ///     Checks if the user's access list contains a specific permission flag.
    // /// </summary>
    // /// <param name="accessLists">Dictionary of access keys to permission flags.</param>
    // /// <param name="accessKey">The category ID (e.g., 1 for ProjectAccess).</param>
    // /// <param name="requiredFlags"></param>
    // /// <returns>True if access is granted; otherwise false.</returns>
    // public static bool HasAccess(
    //     this Dictionary<int, AccessPermission> accessLists,
    //     int accessKey,
    //     params AccessPermission[] requiredFlags)
    // {
    //     if (!accessLists.TryGetValue(accessKey, out var permissionValue))
    //         return false;
    //
    //     return requiredFlags.Length == 0 || requiredFlags.All(required => permissionValue.HasFlag(required));
    // }

    // public static bool HasAccess(this Dictionary<int, AccessPermission> accessLists, params int[] accessKeys)
    // {
    //     return accessKeys.Any(key => accessLists.TryGetValue(key, out _));
    // }
}