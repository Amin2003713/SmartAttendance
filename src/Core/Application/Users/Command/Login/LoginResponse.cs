using Shifty.Domain.Enums;

namespace Shifty.Application.Users.Command.Login
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public UserInfo UserInfo { get; set; }
    }
}
