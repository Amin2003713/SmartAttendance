using MediatR;

namespace Shifty.Application.Users.Command.Login
{
    public class AdminsLoginCommand : IRequest<LoginResponse>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Refresh_token { get; set; }
    }
}