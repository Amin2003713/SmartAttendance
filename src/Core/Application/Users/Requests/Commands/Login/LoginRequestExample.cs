using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Users.Requests.Commands.Login
{
    public class LoginRequestExample : IExamplesProvider<LoginRequest>
    {
        public LoginRequest GetExamples()
        {
            return new LoginRequest
            {
                Username = "09134041709" , Password = "@Shifty403" ,
            };
        }
    }
}