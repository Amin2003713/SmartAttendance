namespace Shifty.Application.Features.Users.Requests.Commands.Login;

public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples()
    {
        return new LoginRequest
        {
            UserName = "09134041709",
            Password = "@NimaDrp1"
        };
    }
}