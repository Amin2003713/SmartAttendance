namespace Shifty.Application.Users.Requests.Login;

public class LoginRequest
{
    public string Username { get; set; }

    public string Password { get; set; }

    public string? Refresh_token { get; set; }
}

