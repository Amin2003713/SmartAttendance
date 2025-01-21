using System;

namespace Shifty.Domain.Enums
{
    [Flags]
    public enum UserRoles : byte
    {
        None        = 0 ,
        Admin       = 1 << 0 , // 1
        Employee    = 1 << 1 , // 2
        Driver      = 1 << 2 , // 4
        Accountants = 1 << 3 , // 8
    }
}