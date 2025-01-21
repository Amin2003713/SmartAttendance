namespace Shifty.Application.Users.Exceptions
{
    public static class UserExceptions
    {
        public static string User_Already_Exists     = nameof(User_Already_Exists);
        public static string Verify_Two_Factor_Token = nameof(Verify_Two_Factor_Token);
        public static string InCorrect_User_Password = nameof(InCorrect_User_Password);
        public const  string User_NotFound           = nameof(User_NotFound);
        public const  string Identity_Error          = nameof(Identity_Error);
        public static string User_Conflict           = nameof(User_Conflict);
        public static string InValide_Token          = nameof(InValide_Token);
    }
}