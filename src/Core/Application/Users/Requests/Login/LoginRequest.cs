using Shifty.Application.Users.Command.Login;
using Shifty.Domain.Users;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Shifty.Application.Users.Requests.Login
{
    public class LoginRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string? Refresh_token { get; set; }
    }

    public class LoginRequestExample : IExamplesProvider<LoginRequest>
    {
        public LoginRequest GetExamples()
        {
            return new LoginRequest
            {
                Username = "admin" , Password = "@Shifty403"
            };
        }
    }
}