namespace Shifty.Application.Features.Users.Requests.Commands.Login;

public class LoginRequest
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string? Refresh_token { get; set; }
}