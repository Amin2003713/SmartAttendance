using Microsoft.Extensions.Localization;
using Shifty.Resources.Common;

namespace Shifty.Resources.Messages
{
    public class CommonMessages(IStringLocalizer<CommonMessages> localizer) : BaseLocalizer<CommonMessages>(localizer)
    {
        public   string Code_Generator()=> Localize(nameof(Code_Generator));
        public   string Server_Error() => Localize(nameof(Server_Error));
        public  string Refresh_Token_Found() => Localize(nameof(Refresh_Token_Found));
        public   string Unauthorized_Access() => Localize(nameof(Unauthorized_Access));


        // Validation Error Messages
        public string Validation_Title() => Localize(nameof(Validation_Title));
        public string Validation_Detail() => Localize(nameof(Validation_Detail));

        // Not Found
        public string NotFound_Title() => Localize(nameof(NotFound_Title));
        public string NotFound_Detail() => Localize(nameof(NotFound_Detail));

        // Internal Server Error
        public string InternalServerError_Title() => Localize(nameof(InternalServerError_Title));

        // Conflict
        public string Conflict_Title() => Localize(nameof(Conflict_Title));
        public string Conflict_Detail() => Localize(nameof(Conflict_Detail));

        // Unauthorized
        public string Unauthorized_Title() => Localize(nameof(Unauthorized_Title));
        public string Unauthorized_Detail() => Localize(nameof(Unauthorized_Detail));

        // Forbidden
        public string Forbidden_Title() => Localize(nameof(Forbidden_Title));
        public string Forbidden_Detail() => Localize(nameof(Forbidden_Detail));



     
            // Shared Messages
            public string Validation_Title_Generic() => Localize(nameof(Validation_Title_Generic));
            public string Validation_Title_NotFound() => Localize(nameof(Validation_Title_NotFound));
            public string Validation_Title_Unauthorized() => Localize(nameof(Validation_Title_Unauthorized));
            public string Validation_Title_Forbidden() => Localize(nameof(Validation_Title_Forbidden));

            // Tenant Validation
            public string Tenant_Error_ParamsMissing() => Localize(nameof(Tenant_Error_ParamsMissing));
            public string Tenant_Error_NotFound() => Localize(nameof(Tenant_Error_NotFound));
            public string Tenant_Tip_Params() => Localize(nameof(Tenant_Tip_Params));

            // Device Validation
            public string Device_Error_MissingHeader() => Localize(nameof(Device_Error_MissingHeader));
            public string Device_Tip_Hardware() => Localize(nameof(Device_Tip_Hardware));

            // Hardware Validation
            public string Hardware_Error_AlreadyRegistered() => Localize(nameof(Hardware_Error_AlreadyRegistered));
            public string Hardware_Error_Mismatch() => Localize(nameof(Hardware_Error_Mismatch));

            public string Device_Tip_DeviceType() => Localize(nameof(Device_Tip_DeviceType));


            public string User_Name_Missing_Tip() => Localize(nameof(User_Name_Missing_Tip));
        
    }
}