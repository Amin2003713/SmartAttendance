namespace Shifty.Common.General.Enums.Projects;

[Flags]
public enum AccessPermission : byte
{
    None   = 0,
    Get    = 1 << 0,
    Post   = 1 << 1 | Get, // 3
    Put    = 1 << 2 | Get, // 5
    Delete = 1 << 3 | Get, // 9


    GranFullAccess = Post | Put | Delete
}