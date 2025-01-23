using Microsoft.Extensions.Localization;
using Shifty.Resources.Common;

namespace Shifty.Resources.Messages
{
    public  class UserMessages(IStringLocalizer<UserMessages> localizer) : BaseLocalizer<UserMessages>(localizer)
    {
        public  string User_Already_Exists     ()=> Localize( nameof(User_Already_Exists));
        public  string Verify_Two_Factor_Token ()=> Localize( nameof(Verify_Two_Factor_Token));
        public  string InCorrect_User_Password ()=> Localize( nameof(InCorrect_User_Password));
        public   string User_NotFound           ()=> Localize( nameof(User_NotFound));
        public   string Identity_Error          ()=> Localize( nameof(Identity_Error));
        public  string User_Conflict           ()=> Localize( nameof(User_Conflict));
        public  string InValid_Token          ()=> Localize( nameof(InValid_Token));

        public string User_Error_NotActive() => Localize(nameof(User_Error_NotActive));

    }
}