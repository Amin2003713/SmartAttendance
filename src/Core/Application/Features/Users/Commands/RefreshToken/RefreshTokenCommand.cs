namespace Shifty.Application.Features.Users.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
{
    public string RefreshToken { get; set; }

    public string AccessToken { get; set; }
}