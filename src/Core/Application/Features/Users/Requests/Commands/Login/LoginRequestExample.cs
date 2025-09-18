namespace SmartAttendance.Application.Features.Users.Requests.Commands.Login;

public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples()
    {
        return new LoginRequest
        {
            UserName = "09131283883",
            Password = "@EsfUni123"
        };
    }
}