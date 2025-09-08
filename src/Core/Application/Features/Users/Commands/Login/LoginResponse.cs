namespace SmartAttendance.Application.Features.Users.Commands.Login;

public class LoginResponse
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }
}