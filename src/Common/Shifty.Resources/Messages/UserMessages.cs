using Microsoft.Extensions.Localization;
using Shifty.Resources.Common;

namespace Shifty.Resources.Messages
{
    public  class UserMessages(IStringLocalizer<UserMessages> localizer) : BaseLocalizer<UserMessages>(localizer)
    {
        public string New_Password_Must_not_be_equal_to_Old_password ()=> localizer[nameof(New_Password_Must_not_be_equal_to_Old_password)];
        public string Password_Change_Error () => localizer[nameof(Password_Change_Error)];
        public string User_Activation_Failed() => Localize(nameof(User_Activation_Failed));
        public  string User_Already_Exists     ()=> Localize( nameof(User_Already_Exists));
        public  string Verify_Two_Factor_Token ()=> Localize( nameof(Verify_Two_Factor_Token));
        public  string InCorrect_User_Password ()=> Localize( nameof(InCorrect_User_Password));
        public   string User_NotFound           ()=> Localize( nameof(User_NotFound));
        public   string Identity_Error          ()=> Localize( nameof(Identity_Error));
        public  string User_Conflict           ()=> Localize( nameof(User_Conflict));
        public  string InValid_Token          ()=> Localize( nameof(InValid_Token));

        public string User_Error_NotActive() => Localize(nameof(User_Error_NotActive));

        public string NotAllowed () => Localize( nameof(NotAllowed));

        public string PhoneNumberRequired() => Localize(nameof(PhoneNumberRequired));
        public string PhoneNumberMinDigits() => Localize(nameof(PhoneNumberMinDigits));

        public string PasswordRequired() => Localize(nameof(PasswordRequired));
        public string PasswordMinLength() => Localize(nameof(PasswordMinLength));
        public string PasswordUppercaseRequired() => Localize(nameof(PasswordUppercaseRequired));
        public string PasswordLowercaseRequired() => Localize(nameof(PasswordLowercaseRequired));
        public string PasswordDigitRequired() => Localize(nameof(PasswordDigitRequired));
        public string PasswordSpecialRequired() => Localize(nameof(PasswordSpecialRequired));

        public string ConfirmPasswordRequired() => Localize(nameof(ConfirmPasswordRequired));
        public string ConfirmPasswordMustMatch() => Localize(nameof(ConfirmPasswordMustMatch));

    }
}