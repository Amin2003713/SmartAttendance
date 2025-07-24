using Shifty.Application.Users.Requests.Commands.Login;

namespace Shifty.Application.Users.Commands.Login;

public class LoginCommand : LoginRequest,
    IRequest<LoginResponse>;