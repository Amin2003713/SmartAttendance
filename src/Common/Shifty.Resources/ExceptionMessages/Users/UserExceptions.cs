using Microsoft.Extensions.Localization;
using Shifty.Resources.ExceptionMessages.Common;
using Shifty.Resources.Resources.Users;

namespace Shifty.Resources.ExceptionMessages.Users
{
    public  class UserMessages(IStringLocalizer<UserResources> localizer) : BaseLocalizer<UserResources>(localizer)
    {
        public  string User_Already_Exists     ()=> Localize( nameof(User_Already_Exists));
        public  string Verify_Two_Factor_Token ()=> Localize( nameof(Verify_Two_Factor_Token));
        public  string InCorrect_User_Password ()=> Localize( nameof(InCorrect_User_Password));
        public   string User_NotFound           ()=> Localize( nameof(User_NotFound));
        public   string Identity_Error          ()=> Localize( nameof(Identity_Error));
        public  string User_Conflict           ()=> Localize( nameof(User_Conflict));
        public  string InValid_Token          ()=> Localize( nameof(InValid_Token));
    }
}