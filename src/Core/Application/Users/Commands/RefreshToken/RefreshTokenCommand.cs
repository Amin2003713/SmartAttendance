namespace Shifty.Application.Users.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
{
    public string RefreshToken { get; set; }

    public string AccessToken { get; set; }
}