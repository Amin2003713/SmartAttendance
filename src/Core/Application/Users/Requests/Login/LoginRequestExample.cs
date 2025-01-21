using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Users.Requests.Login
{
    public class LoginRequestExample : IExamplesProvider<LoginRequest>
    {
        public LoginRequest GetExamples()
        {
            return new LoginRequest
            {
                Username = "admin" , Password = "@Shifty403" ,
            };
        }
    }
}