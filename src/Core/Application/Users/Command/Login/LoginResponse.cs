using System;
using System.Collections.Generic;

namespace Shifty.Application.Users.Command.Login
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}