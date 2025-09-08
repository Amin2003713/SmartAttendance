namespace SmartAttendance.Application.Features.Users.Requests.Commands.Login;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }
}