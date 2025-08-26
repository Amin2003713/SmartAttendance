using Shifty.Application.Features.Users.Requests.Commands.Login;

namespace Shifty.Application.Features.Users.Commands.Login;

public class LoginCommand : LoginRequest,
    IRequest<LoginResponse>;