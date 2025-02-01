using System;

namespace Shifty.Domain.Enums
{
  
    public enum UserRoles : byte
    {
        None        = 0 ,
        Admin       = 1 , // 1
        Employee    = 2 , // 2
        Driver      = 3 , // 4
        Accountants = 4 , // 8
    }
}