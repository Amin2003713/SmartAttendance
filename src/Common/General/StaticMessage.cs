// ReSharper disable InconsistentNaming

using System.Net;

namespace Shifty.Common.General
{
    public static class StaticMessage
    {
        public static string User_Conflict = nameof(User_Conflict);
        public static string Tenant_Admin_Not_Found   = nameof(Tenant_Admin_Not_Found);
        public static string Tenant_Admin_Not_Created = nameof(Tenant_Admin_Not_Created);
        public static string Refresh_Token_Not_Found  = nameof(Refresh_Token_Not_Found);
        public const  string Tenant_Is_Valid          = nameof(Tenant_Is_Valid);
        public const  string Tenant_Is_Not_Valid      = nameof(Tenant_Is_Not_Valid);
        public const  string Server_Error             = nameof(Server_Error);
        public const  string Company_Not_Found        = nameof(Company_Not_Found);
        public const  string Unauthorized_Access      = nameof(Unauthorized_Access);
        public const  string User_NotFound            = nameof(User_NotFound);
        public const  string Identity_Error           = nameof(Identity_Error);
        public const  string Code_Generator           = nameof(Code_Generator);
        public const  string Tenant_Exists            = nameof(Tenant_Exists);
        public const  string Cant_The_Company         = nameof(Cant_The_Company);
    }
}